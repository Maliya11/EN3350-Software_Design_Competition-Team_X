#!/bin/bash
cd /home/ec2-user
aws s3 cp s3://teamxfirstbucket/Backend-0.0.1-SNAPSHOT.jar .
java -jar Backend-0.0.1-SNAPSHOT.jar