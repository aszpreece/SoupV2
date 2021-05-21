from numpy.core.defchararray import array, partition
from numpy.lib.function_base import average
import ijson
import numpy as np
import matplotlib.pyplot as plt

# change this
f = open("./demoStatsSample.soupstats", "r")

objects = ijson.items(f, 'item')

positionReports = (o for o in objects if o['$type'] == "SoupV2.Simulation.Statistics.StatReporting.ReportInfoObjects.SimulationColourInfo, SoupV2")


currentTimeStamp = -1

firstIter = True

# Calculates average variance of the colours of each critter within their species
def calculateColourVar(speciesMap):
    total = np.array([0.0, 0.0, 0.0])
    for memberCols in list(speciesMap.values()):
        npMemberCol = np.array(memberCols)
        r = np.var(npMemberCol[:, 0])
        g = np.var(npMemberCol[:, 1])
        b = np.var(npMemberCol[:, 2])
        total += [r, g, b]

    total /= len(speciesMap.values())
    return total

# list of tuples representing std in r, g, b averaged across all species
coloursStdOverTime = []
coloursOverTime = []

currentSpeciesMap = {}
timeStamps = []

count = 0

colourTotal = np.array([])
for report in positionReports:
    # split clusterings based on timestamp
    if report["TimeStamp"] != currentTimeStamp:  
        if firstIter == False:
            # append
            coloursStdOverTime.append(calculateColourVar(currentSpeciesMap))
            coloursOverTime.append(colourTotal / count)
        currentTimeStamp = report["TimeStamp"]
        print(f'new timestamp: {currentTimeStamp}')
        timeStamps.append(currentTimeStamp)
        firstIter = False
        # reset
        currentSpeciesMap = {}
        colourTotal = np.array([0.0, 0.0, 0.0])
        count = 0

    count += 1    
    colour = np.array(
        [np.array(float(report["R"]))
        , np.array(float(report["G"]))
        , np.array(float(report["B"]))]
    )
    colourTotal += colour
    currentSpeciesMap.setdefault(report["Species"], []).append(colour)

# append straggler
coloursStdOverTime.append(calculateColourVar(currentSpeciesMap))
coloursOverTime.append(colourTotal / count)

print(coloursOverTime)
for i, col in enumerate(coloursOverTime):
    #print(coloursOverTime[i])
    plt.plot(timeStamps[i], 1, marker='s', color=(1+coloursOverTime[i])/2,  markersize=30)

#plt.plot(timeStamps, coloursOverTime)
plt.title("Average Colour Over Time")
plt.xlabel("Time (Seconds)")
plt.ylabel("Average Colour")

plt.show()