from numpy.core.defchararray import partition
import ijson
from itertools import groupby
from operator import itemgetter
from sklearn.neighbors import KDTree
import numpy as np
import matplotlib.pyplot as plt


# change this
f = open("./fixedEnergyStats.soupstats", "r")

objects = ijson.items(f, 'item')

positionReports = (o for o in objects if o['$type'] == 'SoupV2.Simulation.Statistics.StatReporting.SimulationPositionInfo, SoupV2')

# "$type": "SoupV2.Simulation.Statistics.StatReporting.SimulationPositionInfo, SoupV2",
# "Position": "-2386.593, -523.1633",
# "RotationRadians": 0.8083147,
# "EntityId": 126,
# "EntityTag": "DefSoupling",
# "TimeStamp": 30.033335

neighbours = 5

averageClusterForTimeStamp = []
currentTimeStamp = 0

firstIter = True

def calculateClusering(positions):
    positions = np.array(positions)
    kdt = KDTree(positions, leaf_size=30, metric='euclidean')
    f = kdt.query_radius(positions, r=100, return_distance=True)
    total = 0


    for distances in f[1]:
        #print(distances)

        total += np.average(distances)
    total /= len(f[1])
    #print(total)

    return total
        # for each, find k nearest neigbours and calculate average distance squared
        
        
# The array of positions in this current timestamp
currentPositions = []
timeStamps = []
for report in positionReports:
    # split clusterings based on timestamp
    if report["TimeStamp"] != currentTimeStamp:
        if firstIter == False:
            averageClusterForTimeStamp.append(calculateClusering(currentPositions))
            # reset everything
            currentPositions = []
        
        currentTimeStamp = report["TimeStamp"]
        print(f'new timestamp: {currentTimeStamp}')
        print(f'{report["EntityTag"]}')
        timeStamps.append(currentTimeStamp)
        firstIter = False
        
    (xstr, ystr) = report['Position'].split(', ')
    currentPositions.append(np.array([float(xstr), float(ystr)]))

# append straggler
averageClusterForTimeStamp.append(calculateClusering(currentPositions))


print(averageClusterForTimeStamp)
plt.title("Average distance of other critters within 100 world units")
plt.xlabel("Time (seconds)")
plt.ylabel("Distance (world units)")
plt.plot(timeStamps, averageClusterForTimeStamp)
plt.show()