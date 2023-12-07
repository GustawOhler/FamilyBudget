
# FamilyBudget

  

This project is mainly API for managing group budgets. You can create budget, add your friends or family and add various incomes and expenses to get clear view of it. Frontend part of application is in progress.

  

## Usage

  

To setup and start this application you just need few simple steps:

  1. First of all you need to set secret for JWT used for authentication. If you have .NET 7 installed on your CPU you can use my side utility for creating GUIDs to create pretty solid salt:

		    ./.secretStorage/GuidGenerator/GuidGenerator.exe > ./.secretStorage/secrets/JWTSecret.txt

		If you don't have .NET you can set it on your own by creating this file: 

			./.secretStorage/secrets/JWTSecret.txt

2. Create file 

		./.secretStorage/secrets/DbPassword.txt

	and write a strong database password to it.

3. 
		docker-compose build

4. 
		docker-compose up

And you are ready to go!