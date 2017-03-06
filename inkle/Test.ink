-> start

== start ==
Hello, what's on your mind?
+ { not quest } I heard you have trouble with Orcs?
  -> orcs
+ Where can I find herbs around here?
  -> herbs

== orcs ==
Yes, go to the cave and kill them all!
* Sure thing! -> quest
+ No
    Come back when you're ready -> start

== quest ==
Take this sowrd and go now
* [Continue] -> start

== herbs ==
The herbs are in the middle of the dark forest.
+ [Back] -> start
