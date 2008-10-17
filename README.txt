Project was written with Visual Studio 2008 / .NET Framework 3.5

NOTES:
======
All files and assets being used are stored in the root of the project to help save data transfer
to mobile phones.  File names are also as short as possible to help reduce the amount of data sent.

Database can be created with ShackToGo-Create.sql stored in the Assets folder, the database name 
should be ShackToGo or the LINQ data layer will break.  You'll need to update your connection 
information in the web.config. 

To enable the poritions of the site that use user specific function append ?u=username to default url,
for example:

http://your_host/?u=username

