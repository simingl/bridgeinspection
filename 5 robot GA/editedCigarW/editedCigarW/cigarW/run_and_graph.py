'''
Program to run bridge GA a slecected number of times, and
graph average of results.

'''

import matplotlib.pyplot as plt
import math
import random
import numpy as np
import os
import subprocess
from subprocess import Popen, PIPE
import datetime

#Parameters
NUM_RUNS = 5
NUM_GENS = 1000  #This should match what is used in the GA program (decided in infile)

myStart = datetime.datetime.now()

#clear the content of text files where fitness and route data are stored.
open('myData.txt', 'w').close()
open('myRoutes.txt', 'w').close()



#Run Program a set number of times
for x in range(NUM_RUNS):
    
    r = random.uniform(0, 1)  #Random seed passed to the program
    
    print("\nSTARTING RUN " + str(x) + " || Seed: " + str(r) + "\n")
    

    #make command line string to call program w/ arguments
    cmd = ["./cigarW2R", "-i", "infile", "-a", "graph-c3.csv", "-M", "-s", str(r)]

    #Call the program via cmd string
    result = subprocess.Popen(cmd, stdout=subprocess.PIPE)
    out = result.stdout.read()

print("\nGA finished, reading data...\n")

myEnd = datetime.datetime.now()


#Read in data from .txt data files
with open("myData.txt") as infile:
    lines = infile.readlines()

floatLines = []
for x in range(len(lines)):
    floatLines.append(float(lines[x]))

fitness = []
for x in range(len(lines)):
    if len(fitness) < NUM_GENS:
        fitness.append(float(lines[x]))
    else:
        fitness[x % NUM_GENS] += float(lines[x])



for x in range(len(fitness)):
    fitness[x] /= float(NUM_RUNS)
        
print("\nData read in, generating graph.\n")

with open("Run_Info.txt", "w") as outfile:
    m = min(floatLines)
    outfile.write("MIN ACHIEVED: " + str(m) + "\n")
    outfile.write("START: ")
    outfile.write(str(myStart))
    outfile.write("\nFINISH: ")
    outfile.write(str(myEnd))

#Create graphs from collected data
fig, ax = plt.subplots()
#axes = fig.add_axes([0.1, 0.1, 0.8, 0.8]) # left, bottom, width, height (range 0 to 1)

domain = []
for x in range(NUM_GENS):
    domain.append(x)

ax.plot(domain, fitness, label = "Fitness")
    
ax.set_xlabel('Generation')
ax.set_ylabel("Fitness")
    
ax.set_title("2 Robots Minmax")

#ax.legend(loc = 2)

fig.savefig("5R.png")

print("\nGraph saved. Exiting.\n")





 

