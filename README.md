<h3>.Net Core 10 CLI, IMemoryCache and Angular CLI 22</h3>

<h4>.Net Core Key Release Details</h4>
<p>Latest Patch Version: 10.0.9</p>
<p>Corresponding SDK Version: 10.0.301</p>

<h4>Angular CLI 22 Key Release Details and dependencies</h4>
<p>Angular CLI 22.0.O</p>
<p><Tailwind CSS/p>
<p>node v24.15.0</p>
<p>npm v11.12.1</p>


<h4>How to Deploy Angular inside .NetCore 10 project</h4>
<p>1. Navigate to Angular22 root project</p>
<p>2. Execute in terminal<br/>
ng build --configuration=production --base-href=/ </p>
<p>3. It will create dist folder</p>
<p>4. Navigate to dist/browser folder and copy all files to .NetCore project wwwroot folder</p>
<p>5. Copy also server folder, faviicon.ico, 3rdpartylicenses.txt and angular22/dist/angular22/prerendered-routes.json</p>

<h4>How to test and run</h4>
<p>1. From project root directory, navigate to test folder, then run below:</p>
<p>Delete bin and obj folder first <br/>
rm -rf bin/ obj/
</p>
<p>dotnet restore</p>
<p>dotnet test</p>


<h4>How to run project and access it in the browser</h4>
<p>1. Clone repository link </br>
git clone https://github.com/xrey77/CORE10-REST-MEMORY-ANGULAR.git
</p>
<p>2. Navigate to the project root folder</p>
<p> cd CORE10-REST-MEMORY-ANGULAR/core10_memorycache</p>
<p>3. Run the project<br/>
dotnet run
</p>
<p>4. type in the browser address bar: http://localhost:5084</p>


<h4>How to test in Postman</h4>
<p>POST Request     : http://localhost:5084/createcontact</p>
<p>
{
    "firstname": "xxxx",
    "lastname": "xxxx",
    "email": "xxxxx",
    "mobile": "xxxx"    
}
</p>

<p>DELETE Request   : http://localhost:5084/deletecontact/xxx@yahoo.com</p>
<p>GET Request      : http://localhost:5084/getallcontacts</p>
