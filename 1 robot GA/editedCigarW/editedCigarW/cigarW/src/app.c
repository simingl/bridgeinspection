#include <stdio.h>
#include <stdlib.h>
#include <ctype.h>
#include <math.h>
#include "type.h"
#include "utils.h"
#include "random.h"
#include <stdbool.h>

#define INF 0x7f7f7f7f

Coords *cities;

int graph[NUMBER_POINTS][NUMBER_POINTS];
Edges edges[NUMBER_EDGES];
int edgeValues[NUMBER_EDGES];

//To handle deadheading vs. inspection traversal
double visitedGraph[NUMBER_POINTS][NUMBER_POINTS];
double verifyGraph[NUMBER_POINTS][NUMBER_POINTS];
const double DEADHEADING_RATIO = 1.0; 	//Ratio of deadheading cost to service cost (can be adjusted to reflect real speeds of robot)
const int NUM_PERMUTATIONS = 1;
int printCounter = 1000;



int totalLength;

typedef struct {	//struct to return cost and route of vertices at once in Dijkstra
	int dist;
	int route[1500];

} Tuple;

int Dijkstra(int start, int end, int route[1500]);
double TSPDist(ChromType c1, ChromType c2);
void RemoveCrossings(IPTR pj, int start, int end);
void Reverse(IPTR tmp, int lchrom, ChromType c1, ChromType c2);
Tuple routeDistance(IPTR pj, int posStart, int posEnd, int* currentPoint);
Tuple PhenoRouteGet(IPTR pj, int posStart, int posEnd, int* currentPoint);

int Dijkstra(int start, int end, int route[1500])
{
	int dis[NUMBER_POINTS];
	bool vis[NUMBER_POINTS];
	
	int prev[NUMBER_POINTS]; //to track route

	memset(dis, INF, sizeof(int) * NUMBER_POINTS);
	memset(vis, false, sizeof(bool) * NUMBER_POINTS);
	memset(prev, -1, sizeof(int) * NUMBER_POINTS);

	dis[start] = 0;
	vis[start] = true;

	//update the connected distance
	for (int j = 0; j < NUMBER_POINTS; j++) {
		if (vis[j] == false && graph[start][j] > 0) {
			dis[j] = graph[start][j] * visitedGraph[start][j];   //Multiply distance by 1.0 if unvisited, by deadheading ratio if visited -- 

		}
	}

	for (int i = 0; i < NUMBER_POINTS; i++)
	{
		//find the connected point with the shortest distance
		int minx = INF;
		int minmark = 0;
		for (int j = 0; j < NUMBER_POINTS; j++)
		{
			if (vis[j] == false && dis[j] <= minx)
			{
				minx = dis[j];
				minmark = j;
			}
		}
		//mark the point
		vis[minmark] = true;
	
		//update all the unmarked points connected to the current marked point.
		for (int j = 0; j < NUMBER_POINTS; j++)
		{
			if (vis[j] == false && graph[minmark][j] > 0 && dis[j]>dis[minmark] + (graph[minmark][j] * visitedGraph[minmark][j])) //mult with visited graph to account for deadheading -- 
			{
				dis[j] = dis[minmark] + (graph[minmark][j] * visitedGraph[minmark][j]);	//mult with visited graph to account for deadheading --
				prev[j] = minmark;
			}
		}
	}

	//Reconstruct route (produces route in reverse order: destination to source)
	int u = end;
	int routeIndex = 0;
	if (prev[u] != -1 || u == start)
	{
		while (u != -1)
		{
			route[routeIndex] = u;
			routeIndex++;
			u = prev[u];
		}
	}
	for (int i = 0; i < 1500; i++)
	{
		if (route[i] < 0)
		{
			route[i] = -1;
		}
	}


	/*
	printf("\nsequence: ");
	for (int i = 0; i < 1500; i++)
	{
		if (route[i] != -1)
		{
			printf("%d,", route[i]);
		}
	}
	printf("\n");*/

	return dis[end];
}



//Eval one depot starting at point 0
double Eval1R(IPTR pj)
{
	int currentPoint1 = getStartPoint(pj, 0, 1);
	//route distance from 0 to the end
	return routeDistance(pj, 0, pj->chromLen, &currentPoint1).dist;
}

int getStartPoint(IPTR pj, int p0, int p1) {//p0=30, p1=31   -- returning too high value
	int p0s = edges[pj->chrom[p0]].start;//p0s = start point for edge. p0e = end point for edge
	int p0e = edges[pj->chrom[p0]].end;
	int p1s = edges[pj->chrom[p1]].start;
	int p1e = edges[pj->chrom[p1]].end;//posibly going past the max length since robots index >= 83 when asked for robot+1 and robot+2

	if (p0s == p1s || p0s == p1e)  //if 0s=1s or 0s=1e
	{
		return p0e;
	}
	else if (p0e == p1s || p0e == p1e) //if 0e=1s or 0e=1e
	{
		return p0s;
	}
	else 
	{
		int route[1500];
		int d00 = Dijkstra(p0s, p1s, route);//0start to 1start
		int d01 = Dijkstra(p0s, p1e, route);//0start to 1end
		int d10 = Dijkstra(p0e, p1s, route);//0e to 1s
		int d11 = Dijkstra(p0e, p1e, route);//0e to 1e
		int max = max(max(max(d00, d01), d10), d11);//shouldn't this be min?
		if (d00 == max || d01 == max) {//if 0s - 1s or 1e is longest start at 0s
			return p0s;
		}
		else if (d10 == max ||d11==max) {//if 0e - 1s/1e is longest start at 0e
			return p0e;
		}
	}
}

Tuple routeDistance(IPTR pj, int posStart, int posEnd, int* currentPoint)
{

	//store vertices of route in this array
	//int* myRoute;
	int myRoute[1500];
	int dummyRoute[1500];
	int dummyRoute2[1500];

	for (int z = 0; z < 1500; z++)
	{
		myRoute[z] = -1;
		dummyRoute[z] = -1;
		dummyRoute2[z] = -1;
	}
	//myRoute = myChars;
	int routeIndex = 0;

	int currentDistance = 0;
	//first edge
	if (*currentPoint == edges[pj->chrom[posStart]].start)
	{
		*currentPoint = edges[pj->chrom[posStart]].end;
		graph[edges[pj->chrom[posStart]].start][edges[pj->chrom[posStart]].end];

		/*
		myRoute[routeIndex] = edges[pj->chrom[posStart]].start;
		routeIndex++;
		myRoute[routeIndex] = edges[pj->chrom[posStart]].end;
		routeIndex++;*/
	}
	else if (*currentPoint == edges[pj->chrom[posStart]].end)
	{
		*currentPoint = edges[pj->chrom[posStart]].start;
		graph[edges[pj->chrom[posStart]].start][edges[pj->chrom[posStart]].end];

		
		/*
		myRoute[routeIndex] = edges[pj->chrom[posStart]].end;
		routeIndex++;
		myRoute[routeIndex] = edges[pj->chrom[posStart]].start;
		routeIndex++;*/
	}

	//calcluate the distance of robot 1 from 2nd edge to robot 2
	for (int i = posStart + 1; i < posEnd; i++) {
		int dissta = Dijkstra(*currentPoint, edges[pj->chrom[i]].start, dummyRoute);
		int disend = Dijkstra(*currentPoint, edges[pj->chrom[i]].end, dummyRoute2);
		if (dissta < disend)
		{
			currentDistance += dissta;
			*currentPoint = edges[pj->chrom[i]].end;

			for (int z = 0; z < 1500; z++)
			{
				if (dummyRoute[z] != -1)
				{ 
					myRoute[routeIndex] = dummyRoute[z];
					routeIndex++;
				}
			}

			for(int z = 0; z < 499; z++)
			{ 
				if (dummyRoute[z] != -1 && dummyRoute[z + 1] != -1)
				{ 
					//visitedGraph[dummyRoute[z]][dummyRoute[z + 1]] = DEADHEADING_RATIO;  //mark edge as visited
					//visitedGraph[dummyRoute[z + 1]][dummyRoute[z]] = DEADHEADING_RATIO;
				}
			}
			
			
		
		}
		else
		{
			currentDistance += disend;
			*currentPoint = edges[pj->chrom[i]].start;

			for (int z = 0; z < 1500; z++)
			{
				if (dummyRoute2[z] != -1)
				{
					myRoute[routeIndex] = dummyRoute2[z];
					routeIndex++;
				}
			}

			for (int z = 0; z < 499; z++)
			{
				if (dummyRoute2[z] != -1 && dummyRoute2[z + 1] != -1)
				{ 
					//visitedGraph[dummyRoute2[z]][dummyRoute2[z + 1]] = DEADHEADING_RATIO;  //mark edge as visited
					//visitedGraph[dummyRoute2[z + 1]][dummyRoute2[z]] = DEADHEADING_RATIO;
				}
			}

		}
		currentDistance += graph[edges[pj->chrom[i]].start][edges[pj->chrom[i]].end] * visitedGraph[edges[pj->chrom[i]].start][edges[pj->chrom[i]].end];//mult with visited graph to account for deadheading -- 
		//visitedGraph[edges[pj->chrom[i]].start][edges[pj->chrom[i]].end] = DEADHEADING_RATIO;   //mark edge as visited
		//visitedGraph[edges[pj->chrom[i]].end][edges[pj->chrom[i]].start] = DEADHEADING_RATIO;   //mark edge as visited in reverse direction

		for (int z = 0; z < 1500; z++)
		{
			dummyRoute[z] = -1;
			dummyRoute2[z] = -1;
		}
		
		/*
		if (i == posStart + 1)
		{
			myRoute[routeIndex] = edges[pj->chrom[i]].start;
			routeIndex++;
		}*/
		
		myRoute[routeIndex] = edges[pj->chrom[i]].start;
		routeIndex++;

		myRoute[routeIndex] = edges[pj->chrom[i]].end;
		routeIndex++;
	}
	//return currentDistance;

	Tuple result;
	result.dist = currentDistance;

	for (int z = 0; z < 1500; z++)
	{ 
		result.route[z] = myRoute[z];
	}

	//clean route of duplicates
	for (int x = 0; x < 2; x++)
	{ 
		for (int i = 1; i < 1500; i++)
		{
			if (result.route[i - 1] == result.route[i])
			{
				for (int j = i; j < 499; j++)
				{
					result.route[j] = result.route[j + 1];
				}
			}
		}
	}

	/*
	printf("\nroute: ");
	for (int z = 0; z < 1500; z++)
	{
		if (result.route[z] != -1)
		{
			printf("%d, ", result.route[z]);
		}
	}
	printf("\n");*/


	return result;
}


void getRobot12(IPTR pj, int* robot1) {
	for (int i = 0; i < pj->chromLen; i++) {
		if (pj->chrom[i] == pj->chromLen - 1)
		{
			*robot1 = i;
		}
	}
}

//Eval 2 robots starting 0-82 edges, 83, 84, are 2 robots
double Eval(IPTR pj)
{
	//return Eval1R(pj);
	double routeDistTotal = 0.0;
	for (int x = 0; x < NUM_PERMUTATIONS; x++)
	{
		//Reset Visited Graph to unvisited on all edges between evaluations -- 
		for (int i = 0; i < NUMBER_POINTS; i++)
		{
			for (int j = 0; j < NUMBER_POINTS; j++)
			{
				visitedGraph[i][j] = 1.0;	//unvisited
				verifyGraph[i][j] = 0;	//unvisited
			}
		}
		

		double robotDists[2];


		int robot1 = 0;
		getRobot12(pj, &robot1);

		//2 robots are connected, return a low fitness.
		if ( robot1 == 1 ) {
			return 9999999;
		}

		int currentPoint1, currentPoint2;
		double robot1Dis;
		
		//printf("\n-----\n");
		for (int y = 0; y < 1; y++)
		{
			if (y == 0)
			{
				currentPoint1 = getStartPoint(pj, 0, 1);
				//route distance from robot5 to chromLen, part 1

				/*
				double chromDist = 0.0;
				int index = (robot5 + 1) % (pj->chromLen);
				while (index != robot1)
				{
					chromDist += edgeValues[pj->chrom[index]];
					index = (index + 1) % (pj->chromLen);
				}*/
				robot1Dis = 0;
				
				Tuple r1part1;
				int* fullRoute1part1;
				bool part1Flag = false;
				bool part2Flag = false;
				//if robot1 is the last element in the chromosome, there won't be a part1
				//if (robot1 != pj->chromLen - 1)
			
				//r2part1 = routeDistance(pj, (robot2 + 1) % pj->chromLen, pj->chromLen, &currentPoint2);
				r1part1 = PhenoRouteGet(pj, 0,pj ->chromLen, &currentPoint1);
				robot1Dis += r1part1.dist;
				fullRoute1part1 = r1part1.route;
				part1Flag = true;
				

				/*
				Tuple r1part2;
				int* fullRoute1part2;

				//if robot1 is the first element in the chromosome, there won't be a part2
				if (robot1 != 0)
				{
					currentPoint2 = getStartPoint(pj, 0, 1);

					//route distance from 0 to robot1, part 2
					//r5part2 = routeDistance(pj, 0, robot1, &currentPoint5);
					r1part2 = PhenoRouteGet(pj, 0, robot1, &currentPoint2);
					robot1Dis += r1part2.dist;
					fullRoute1part2 = r1part2.route;
					part2Flag = true;
				}
				
				
				if (printCounter % 1000 == 0)
				{
					printf("\n routeDist: %f \n", robot1Dis);
					printf("\n chromSegment: ");
					int index = (robot1 + 1) % (pj->chromLen);
					while (index != robot1)
					{
						printf("%d ", pj->chrom[index]);
						printf("( %d, %d ), ", edges[pj->chrom[index]].start, edges[pj->chrom[index]].end);
						index = (index + 1) % (pj->chromLen);
					}
					printf("\n\n route1: ");
					if (part1Flag == true)
					{
						for (int z = 0; z < 1500; z++)
						{
							if (fullRoute1part1[z] != -1)
							{
								printf("%d,", fullRoute1part1[z]);
							}
						}
					}
					printf(" | ");
					if (part2Flag == true)
					{
						for (int z = 0; z < 1500; z++)
						{
							if (fullRoute1part2[z] != -1)
							{
								printf("%d,", fullRoute1part2[z]);
							}
						}
					}
					printf("\n");
					printf("Full Chrom: ");
					for (int i = 0; i < pj->chromLen; i++)
					{
						printf("%d,", pj->chrom[i]);
					}
					printf("\n");


				}*/

				//printf("\nRoute5: Dist: %f ||", robot5Dis);

				/*
				printf("\nRoute5: ");
				if (partFlag == true)
				{
					for (int z = 0; z < 1500; z++)
					{
						if (fullRoute5part1[z] != -1)
						{
							printf("%d,", fullRoute5part1[z]);
						}
					}
				}
				for (int z = 0; z < 1500; z++)
				{
					if (fullRoute5part2[z] != -1)
					{
						printf("%d,", fullRoute5part2[z]);
					}
				}
				printf("\n");*/
				
			}
			else
			{
				printf("Something went wrong with route ordering...");
			}	 
		}

		//printf("\n-----\n");

		routeDistTotal += robot1Dis;


		//Verify routes are a complete covering
		for (int i = 0; i < NUMBER_POINTS; i++)
		{
			for (int j = 0; j < NUMBER_POINTS; j++)
			{
				if (graph[i][j] > 0)	
				{ 
					if( verifyGraph[i][j] == 0) //a valid edge was not visited, return very low fitness
					{
						return 9999999;
					}
					
				}
			}
		}

	}
	
	double myFitness = (routeDistTotal / NUM_PERMUTATIONS);

	
	return myFitness;

	printCounter++;
}


double TSPDist(ChromType c1, ChromType c2)
{
  double xd, yd;
  xd = (double) (cities[c1].x - cities[c2].x);
  yd = (double) (cities[c1].y - cities[c2].y);
  return rint(sqrt((xd * xd) + (yd * yd)));

}


void AppInitChrom(IPTR pj)
{
  Shuffle(pj->chrom, pj->chromLen);
  
  return;
}

void AppSkipline(FILE *fp)
{
  int ch; 
  while( (ch = fgetc(fp)) != '\n'){
    if(ch == EOF) {
      fprintf(stderr, "AppSkipline: Premature end of file \n");
      exit(1);
    }
  }
}

void AppInit(char *appInfile, Population *p)
{

	FILE *fp;
	if ((fp = fopen(appInfile, "r")) == NULL) {
		fprintf(stderr, "AppInit: Cannot open %s for reading\n", appInfile);
		exit(1);
	}
	int value;
	int edge_index = 0;
	for (int r = 0; r < NUMBER_POINTS; r++) 
	{
		for (int c = 0; c < NUMBER_POINTS; c++) 
		{
			if (c != NUMBER_POINTS - 1) 
			{
				fscanf(fp, "%d,", &value);
			}
			else 
			{
				fscanf(fp, "%d\n", &value);
			}
			graph[r][c] = value;

			if (value > 0 && r < c) {
				edges[edge_index].start = r;
				edges[edge_index].end = c;
				edgeValues[edge_index] = value;
				edge_index++;
				totalLength += value;

				
			}
		}
	}
}


void AppInitTSP(char *appInfile, Population *p)
{
  int i, c, tmp;
  FILE *fp;
  cities = (Coords *) calloc ((size_t) p->chromLength, sizeof(Coords));
  if ((fp = fopen(appInfile, "r")) == NULL) {
    fprintf(stderr, "AppInit: Cannot open %s for reading\n", appInfile);
    exit(1);
  }
  while( isalpha(c = fgetc(fp))){ // skip header 
    AppSkipline(fp);
  }
  ungetc(c, fp);

  // read coords
  for(i = 0; i < p->chromLength; i++){
    fscanf(fp, "%d %f %f", &tmp, &(cities[i].x), &(cities[i].y));
  }
  //  for(i = 0; i < p->chromLength; i++){
  //    fprintf(stdout, "%d %f %f \n", i, (cities[i].x), (cities[i].y));
  //  }

}

//route construct
void PhenoRoutePrint(IPTR pj, int posStart, int posEnd, FILE *fp, int* currentPoint)
{
	int dummyRoute[1500];
	int dummyRoute2[1500];
	for (int i = 0; i < 1500; i++)
	{
		dummyRoute[i] = -1;
		dummyRoute2[i] = -1;
	}

	//first point
	fprintf(fp, "%d,", *currentPoint);

	//first edge
	if (*currentPoint == edges[pj->chrom[posStart]].start)
	{
		*currentPoint = edges[pj->chrom[posStart]].end;
		fprintf(fp, "%d,", *currentPoint);
	}
	else if (*currentPoint == edges[pj->chrom[posStart]].end)
	{
		*currentPoint = edges[pj->chrom[posStart]].start;
		fprintf(fp, "%d,", *currentPoint);
	}

	//calcluate the distance of robot 1 from 2nd edge to robot 2
	for (int i = posStart + 1; i < posEnd; i++) {
		int dissta = Dijkstra(*currentPoint, edges[pj->chrom[i]].start, dummyRoute);
		int disend = Dijkstra(*currentPoint, edges[pj->chrom[i]].end, dummyRoute2);
		if (dissta < disend)
		{
			if (dissta != 0) 
			{
				for (int j = 499; j >= 0; j--)
				{
					if(dummyRoute[j] != -1)
					{ 
						fprintf(fp, "%d,", dummyRoute[j]);
					}
				}
				fprintf(fp, "%d,", edges[pj->chrom[i]].start);
			}
			*currentPoint = edges[pj->chrom[i]].end;
			fprintf(fp, "%d,", *currentPoint);
		}
		else
		{
			if (disend != 0)
			{
				//fprintf(fp, "(");
				for (int j = 499; j >= 0; j--)
				{
					if (dummyRoute2[j] != -1)
					{
						fprintf(fp, "%d,", dummyRoute2[j]);
					}
				}
				//fprintf(fp, ")");
				fprintf(fp, "%d,", edges[pj->chrom[i]].end);
			}
			*currentPoint = edges[pj->chrom[i]].start;
			fprintf(fp, "%d,", *currentPoint);
		}
		for (int j = 0; j < 1500; j++)
		{
			dummyRoute[j] = -1;
			dummyRoute2[j] = -1;
		}
	}
}

//function to return route and cost of one section of genome (1 robot's path)
Tuple PhenoRouteGet(IPTR pj, int posStart, int posEnd, int* currentPoint)
{
	Tuple myTuple;

	//printf("posStart: %d\n", posStart);
	//printf("posEnd: %d\n", posEnd);
	//printf("curr: %d\n", currentPoint);

	if (posStart == posEnd)
	{
		myTuple.dist = 9999999;
		return myTuple;
	}

	int myRoute[1500];
	int routeIndex = 0;

	int dummyRoute[1500];
	int dummyRoute2[1500];
	for (int i = 0; i < 1500; i++)
	{
		dummyRoute[i] = -1;
		dummyRoute2[i] = -1;
		myRoute[i] = -1;
	}

	//first point
	//fprintf(fp, "%d,", *currentPoint);
	myRoute[routeIndex] = *currentPoint;
	routeIndex++;

	//first edge
	if (*currentPoint == edges[pj->chrom[posStart]].start)
	{
		*currentPoint = edges[pj->chrom[posStart]].end;
		//fprintf(fp, "%d,", *currentPoint);
		myRoute[routeIndex] = *currentPoint;
		routeIndex++;
	}
	else if (*currentPoint == edges[pj->chrom[posStart]].end)
	{
		*currentPoint = edges[pj->chrom[posStart]].start;
		//fprintf(fp, "%d,", *currentPoint);
		myRoute[routeIndex] = *currentPoint;
		routeIndex++;
	}

	//calcluate the distance of robot 1 from 2nd edge to robot 2
	for (int i = posStart + 1; i < posEnd; i++) {
		int dissta = Dijkstra(*currentPoint, edges[pj->chrom[i]].start, dummyRoute);
		int disend = Dijkstra(*currentPoint, edges[pj->chrom[i]].end, dummyRoute2);
		if (dissta < disend)
		{
			if (dissta != 0)
			{
				for (int j = 499; j >= 0; j--)
				{
					if (dummyRoute[j] != -1)
					{
						//fprintf(fp, "%d,", dummyRoute[j]);
						myRoute[routeIndex] = dummyRoute[j];
						routeIndex++;
					}
				}
				//fprintf(fp, "%d,", edges[pj->chrom[i]].start);
				myRoute[routeIndex] = edges[pj ->chrom[i]].start;
				routeIndex++;
			}
			*currentPoint = edges[pj->chrom[i]].end;
			//fprintf(fp, "%d,", *currentPoint);
			myRoute[routeIndex] = *currentPoint;
			routeIndex++;
		}
		else
		{
			if (disend != 0)
			{
				//fprintf(fp, "(");
				for (int j = 499; j >= 0; j--)
				{
					if (dummyRoute2[j] != -1)
					{
						//fprintf(fp, "%d,", dummyRoute2[j]);
						myRoute[routeIndex] = dummyRoute2[j];
						routeIndex++;
					}
				}
				//fprintf(fp, ")");
				//fprintf(fp, "%d,", edges[pj->chrom[i]].end);
				myRoute[routeIndex] = edges[pj ->chrom[i]].end;
				routeIndex++;
			}
			*currentPoint = edges[pj->chrom[i]].start;
			//fprintf(fp, "%d,", *currentPoint);
			myRoute[routeIndex] = *currentPoint;
			routeIndex++;
		}
		for (int j = 0; j < 1500; j++)
		{
			dummyRoute[j] = -1;
			dummyRoute2[j] = -1;
		}
	}

	for (int i = 0; i < 1500; i++)
	{
		myTuple.route[i] = myRoute[i];
	}

	//clean route of duplicates
	for (int x = 0; x < 2; x++)
	{
		for (int i = 1; i < 1500; i++)
		{
			if (myTuple.route[i - 1] == myTuple.route[i])
			{
				for (int j = i; j < 499; j++)
				{
					myTuple.route[j] = myTuple.route[j + 1];
				}
			}
		}
	}

	/*
	printf("\n\n");
	for (int i = 0; i < 1500; i++)
	{
		if (myTuple.route[i] != -1)
		{
			printf("%d,", myTuple.route[i]);
		}
	}
	printf("\n\n");*/

	double myDist = 0.0;

	//calculate cost of route
	for (int i = 0; i < 499; i++)
	{
		if (myTuple.route[i] != -1 && myTuple.route[i + 1] != -1)
		{
			myDist += graph[myTuple.route[i]][myTuple.route[i + 1]] * visitedGraph[myTuple.route[i]][myTuple.route[i + 1]];  //Add distance
			visitedGraph[myTuple.route[i]][myTuple.route[i + 1]] = DEADHEADING_RATIO;  //Mark edge as visited
			//printf("\n | %d, %d | \n", myTuple.route[i], myTuple.route[i + 1]);
			visitedGraph[myTuple.route[i + 1]][myTuple.route[i]] = DEADHEADING_RATIO;
			verifyGraph[myTuple.route[i]][myTuple.route[i + 1]] = 1;
			verifyGraph[myTuple.route[i + 1]][myTuple.route[i]] = 1;
		}
	}

	myTuple.dist = myDist;
	return myTuple;
}

//Single depot
void PhenoPrint1R(FILE *fp, IPTR pop, Population *p)
{
	int i;

	fprintf(fp, "%i ", (int)pop[p->maxi].objfunc);

	for (i = 0; i < p->chromLength; i++) {
		if (pop[p->maxi].chrom[i] >= NUMBER_EDGES) {
			fprintf(fp, " Robot-%d=>", pop[p->maxi].chrom[i]);
		}
		else {
			fprintf(fp, "(E%d%d)", edges[pop[p->maxi].chrom[i]].start, edges[pop[p->maxi].chrom[i]].end);
		}
	}

	fprintf(fp, "\n");

	//print points only
	int currentPoint1 = getStartPoint(pop, 0, 1);
	//route distance from robot1+1 to robot2
	PhenoRoutePrint(pop, 0, pop->chromLen, fp, &currentPoint1);

	fprintf(fp, "\n");
}

//2 robots
void PhenoPrint(FILE *fp, IPTR pop, Population *p)//modified
{
  int i;
  
  fprintf(fp, "%i ", (int) pop[p->maxi].objfunc);

  for(i = 0; i < p->chromLength; i++){
	  if (pop[p->maxi].chrom[i] >= NUMBER_EDGES) {
		  fprintf(fp, " Robot-%d=>", pop[p->maxi].chrom[i]);
	  }
	  else {
		  fprintf(fp, "(E%d%d)", edges[pop[p->maxi].chrom[i]].start, edges[pop[p->maxi].chrom[i]].end);
	  }
  } 

  fprintf(fp, "\n"); 

  //print points only
  int robot1 = 0;
  getRobot12(pop, &robot1);


  int currentPoint1 = getStartPoint(pop, 0, 1);
  fprintf(fp, "\nRobot-1=>");

  PhenoRoutePrint(pop, 0,pop -> chromLen, fp, &currentPoint1);

  fprintf(fp, "\n");
}


void TourPrint(FILE *fp, IPTR pj, char *name)
{
  int i;
  fprintf(fp, "%s :", name); 
  for(i = 0; i < pj->chromLen; i++){
    fprintf(fp, "%i ", pj->chrom[i]);
  } 
  fprintf(fp, "\n"); 
}
  


void InitPhenoPrint(IPTR pj, char *fname, Population *p)
{
  FILE *fp;
  int i;

  if((fp = fopen(fname, "w")) == NULL){
    fprintf(stderr, "InitPhenoPrint: Cannot open %s for writing\n", fname);
    exit(1);
  }
  fprintf(fp, "%i \nTourLength ", (p->chromLength + 1) * 2 + 1);
  for(i = 0; i < p->chromLength; i++){
    fprintf(fp, "x y ");

  }
  fprintf(fp, "x y ");
  fprintf(fp, "\n");
  fclose(fp);
}

void RemoveCrossings(IPTR pj, int start, int end)
{
  int i,j;
  int lchrom;
  IPTR tmp;
  ChromType city1Next, city2Prev;
  
  lchrom = pj->chromLen;
  tmp = AllocateIndividuals(1, lchrom);

  IndividualCopy(pj, tmp);
  for(i = start; i < end; i++){
    for(j = (i+3)%lchrom; j != i; j = ((j+1)%lchrom)){
      city1Next = (i + 1) % lchrom;
      city2Prev = (j + lchrom - 1) % lchrom;
	  if((TSPDist(tmp->chrom[i], tmp->chrom[city1Next])  + TSPDist(tmp->chrom[j], tmp->chrom[city2Prev])) >  (TSPDist(tmp->chrom[i], tmp->chrom[city2Prev]) + TSPDist(tmp->chrom[j], tmp->chrom[city1Next]))) {
 	     Reverse(tmp, lchrom, city1Next, city2Prev);
      }
    }
  }
  IndividualCopy(tmp, pj);
  free(tmp->chrom);
  free(tmp->backup);
  free(tmp);
  return;
}

void Reverse(IPTR tmp, int lchrom, ChromType c1, ChromType c2)
{
  SwapChromType(&(tmp->chrom[c1]), &(tmp->chrom[c2]));
  /****
  do {
    SwapChromType(&(tmp->chrom[c1]), &(tmp->chrom[c2]));
    c2 = (c2 + lchrom - 1) % lchrom;
    c1 = (c1 + 1) % lchrom;
  } while (c2 != c1 && ((c1 + lchrom - 1 ) % lchrom) != c2);
  ****/
return;
}
  
  
  
