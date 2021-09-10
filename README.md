# Convert Database first into Code first in the Abp Framework
> Preface: *Abp suite is known as a tool to generate Code first; however, we also want to aply this tool for Database first, so what should we do?*
> 
> Use for Oracle database.
> 
> Download source: https://github.com/ngotruongblog/Generator


## Step 1: Install EF Core Power Tool for visual studio
![image](https://user-images.githubusercontent.com/45006046/132877440-2550c81e-5597-4829-b082-af80d93ad224.png)
## Step 2: Right-click in Generater project, choose Reverse Engineer
![image](https://user-images.githubusercontent.com/45006046/132877931-5b078402-e1ef-43b6-acd8-0fce2fa5651d.png)
## Step 3: Click the Add Connection icon in Server Explorer
![image](https://user-images.githubusercontent.com/45006046/132878224-8f0d59a3-4334-46b3-8cd1-30ca44c1b867.png)
> Choose Database Connection -> Choose table/view to generate
> 
> ![image](https://user-images.githubusercontent.com/45006046/132878685-13dbf5fb-365c-4a0d-89c0-674f94cc2fba.png)
> 
![image](https://user-images.githubusercontent.com/45006046/132878729-c89b867a-3396-40f1-b738-4014b614d426.png)
## Step 4: Install oracle.entityframeworkcore nuget and delete DataContext.cs file
![image](https://user-images.githubusercontent.com/45006046/132878921-db683ed4-e8e1-49d0-98b9-679750c2b278.png)
## Step 5: Create file json
![image](https://user-images.githubusercontent.com/45006046/132878991-d330b694-4b9d-4017-9431-2e09b959a76b.png)
>In which,
>1. _nameSpaceModels: namespace name of prior created entity
>![image](https://user-images.githubusercontent.com/45006046/132879712-bf1422c0-fb15-46a4-9cf6-404be9388a77.png)
>2. _pathOutput: where contains the json files
>3. _nameProject: module name

## Step 6: Copy json files into .suite\entities\ folder in the Abp project
![image](https://user-images.githubusercontent.com/45006046/132880939-cd43eab0-7ec7-4187-bd50-d84c9b9b9833.png)
>Then, refresh Abp Suite tool and generate code.
## Step 7: After generating code from Abp Suite, we need to edit xxxDbContextModelCreatingExtensions.cs so that the colume name could be fit with database
![image](https://user-images.githubusercontent.com/45006046/132881856-f5573885-6f18-4d60-9060-65b996fa88df.png)

