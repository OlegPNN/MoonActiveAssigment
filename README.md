# MoonActiveAssigment

Your task is to write a simple application server that prints a message at a given time in the future.

The server has only 1 API:
echoAtTime - which receives two parameters, time and message, and writes that message to the server console at the given time.
Since we want the server to be able to withstand restarts it will use Redis to persist the messages and the time they should be sent at.
You should also assume that there might be more than one server running behind a load balancer (load balancing implementation itself does not need to be provided as part of the answer).
In case the server was down when a message should have been printed, it should print it out when going back online.



The application should preferably be written in Node.js. 


The focus of the exercise is:

the efficient use of Redis and its data types
messages should not be lost
message should be printed once
the same message should be printed only once
message order should not be changed
should be scalable
seeing your code in action (SOLID would be a plus)
use only redis.io

# Clarification for task
1) One of the instances should print the message.
2) It’s a mistake. Each API call should correspond to one print in the future.
3) The order of received API calls should correspond to the order of printed messages (if the timestamp is equal).


