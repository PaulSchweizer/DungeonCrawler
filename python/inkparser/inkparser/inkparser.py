inkle = """
=== start ==
Hello, what's on your mind?
+ { not quest } { herbs } I heard you have trouble with Orcs?
  -> orcs
+ Where can I find herbs around here?
  -> herbs

== orcs ==
Yes, go to the cave and kill them all!
* Sure thing! -> quest
+ No
    Come back when you're ready -> start

== quest ==
Take this sword and go now
* [Continue] -> start

== herbs ==
The herbs are in the middle of the dark forest.
+ [Back] -> start

"""

"""

== Knot

+ Option {'Sticky': True}
* Option {'Sticky': False}
+ [Back] Don't display the text in the next section
-> Destination
"""
import re
import json


class InkParser(object):

    """docstring for InkParser"""

    KNOT_ID_RE = r'(=+.*\b.*=+)+'
    KNOT_TEXT_RE = r'^([^\+*]*)'

    OPTIONS_RE = r'([+*].*\n*.*->.*)+'
    DESTINATION_RE = r'(->?(.*))'
    OPTION_TEXT_RE = r'(?<=[+*])(.*)(?=(->))'
    CONDITIONS_RE = r'(?<={)([^}]*)(?=})'

    def parse(self, inkle):
        """"""
        dialog = list()

        # Find and construct the knots
        knot_ids = re.findall(self.KNOT_ID_RE, inkle)
        for i, knot_id in enumerate(knot_ids):
            knot = self._new_knot()
            knot['Id'] = knot_id.replace('=', '').rstrip().lstrip()

            # Filter out the block of text that belongs to the knot
            if i == len(knot_ids)-1:
                content = inkle.split(knot_ids[i])[1]
            else:
                content = inkle.split(knot_ids[i])[1].split(knot_ids[i+1])[0]
            # end if

            # Text
            knot['Text'] = re.findall(self.KNOT_TEXT_RE, content)[0]

            # Options
            option_content = content.split(knot['Text'])[1]
            for option_text in re.findall(self.OPTIONS_RE, option_content):
                option = self._new_option()
                option['Destination'] = re.findall(self.DESTINATION_RE, option_text)[0][1].lstrip()
                option['Text'] = re.findall(self.OPTION_TEXT_RE, option_text, re.DOTALL)[0][0].rstrip().lstrip()
                if option_text.rstrip().startswith('+'):
                    option['Sticky'] = True
                # end if

                option['Id'] = '{0}_{1}'.format(knot['Id'], option['Destination'])

                conditions = re.findall(self.CONDITIONS_RE, option_text)

                if conditions:
                    option['Text'] = option['Text'].split('}')[-1]

                for condition in conditions:
                    condition = condition.rstrip().lstrip()
                    parts = condition.split(' ')
                    # The Condition has to be True
                    if len(parts) == 1:
                        option['Conditions'][condition] = True
                    elif len(parts) == 2:
                        if parts[0] == 'not':
                            option['Conditions'][parts[1]] = False
                        else:
                            raise Exception('Could not parse Condition: {0}'.format(condition))
                        # end if
                    else:
                        raise Exception('Could not parse Condition: {0}'.format(condition))
                    # end if
                # end for

                dialog.append(option)
                knot['Options'].append(option['Id'])
            # end for

            dialog.append(knot)
        # end for

        print json.dumps(dialog, indent=2)

    # end def parse

    def _new_knot(self):
        """@todo documentation for _new_knot."""
        return {
            'Id': None,
            'Type': 'Knot',
            'Text': None,
            'Options': list()
            }
    # end def _new_knot

    def _new_option(self):
        """@todo documentation for _new_option."""
        return {
            'Id': None,
            'Type': 'Option',
            'Text': None,
            'Destination': None,
            'Conditions': dict(),
            'Sticky': False
            }
    # end def _new_option

# end class InkParser


parser = InkParser()

parser.parse(inkle)
