# HOW TO GENERATE DOCUMENTATION USING DocFX:

## Use DocFX integrated with Visual Studio

Step 3 (ignore the rest) https://dotnet.github.io/docfx/tutorial/docfx_getting_started.html


## Add DocFX.exe to system values

After the VS standard plugin is installed the docfx.exe will be in C:\Users\USERNAME\.nuget\packages\docfx.console\2.57.2\tools
you should add it to the path in the Env_Variables (recommended System Variables but optional)

https://windowsloop.com/add-environment-variable-in-windows-10/#:~:text=Steps%20to%20Add%20Environment%20Variable%20in%20Windows%2010,Windows%2010%20to%20apply%20the%20new%20environment%20variable.

Restart is required!


## Build solution it will create the following folders and files in each project in the solution (and all files in them):

|+_site -> where the final website resides
|+api
|+apidoc
|+images
|+articles
|+docfx.json
|+index.md
|+log.txt
|+toc.yml


## Test the generated documentation 

In the terminal or powersheel

``docfx "C:\Users\g_n_p\source\repos\GamificationAPI\GamificationAPI\docfx.json" --serve``


##Serve the site in IIS

The _site has to be placed somewhere like in the wwwrot folder of IIS to be acessed as it will not work properly opened from index.html fule due to CORS security policies.

https://tecadmin.net/create-website-in-iis/


## Generate Static website

To generate static content in docfx.json replace before build and --serve

    "template": [
      "default"
    ],
	
	with 

    "template": [
      "statictoc"
    ],


