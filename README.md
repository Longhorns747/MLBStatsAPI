# MLBStatsAPI
A free, open-source API for MLB statistics. Statistics come from the open-source Sean Lahman database, updated yearly.

Base URL: mlbstatsapi.azurewebsites.net/api/

## Endpoints

###Team Data

/api/teams/teamID (EX. /api/teams/bos, /api/teams/bal, /api/teams/ana)

Sample Response Structure

{"yearRecords":
  {"2014":{"W":"71","L":"91","R":"634","RA":"715","HR":"123","FP":"0.985","AVG":"0.244","ERA":"4.01"},
  "2013":{"W":"97","L":"65","R":"853","RA":"656","HR":"178","FP":"0.987","AVG":"0.277","ERA":"3.79"},
  "2012":{"W":"69","L":"93","R":"734","RA":"806","HR":"165","FP":"0.983","AVG":"0.260","ERA":"4.70"},
  "2011":{"W":"90","L":"72","R":"875","RA":"737","HR":"203","FP":"0.985","AVG":"0.280","ERA":"4.20"}, ...},
  "name":"Boston Red Sox"}

TeamIDs follow the conventions held in Sean Lahman's database.

###Player Data

####Batting Data

/api/batters?firstname=FIRST_NAME&lastname=LAST_NAME (EX. /api/batters?firstname=david&lastname=ortiz)

Sample Response Structure

{"yearRecords":
  {"1997":{"teamID":"MIN","G":"15","AB":"49","H":"16","HR":"1","RBI":"6","SB":"0","AVG":"0.327","OBP":"0.353","SLG":"0.449"},
  "1998":{"teamID":"MIN","G":"86","AB":"278","H":"77","HR":"9","RBI":"46","SB":"1","AVG":"0.277","OBP":"0.371","SLG":"0.446"},
  "1999":{"teamID":"MIN","G":"10","AB":"20","H":"0","HR":"0","RBI":"0","SB":"0","AVG":"0.000","OBP":"0.200","SLG":"0.000"},
  "2000":{"teamID":"MIN","G":"130","AB":"415","H":"117","HR":"10","RBI":"63","SB":"1","AVG":"0.282","OBP":"0.364","SLG":"0.446"}...} ,
  "nameLast":"Ortiz","nameFirst":"David"}

Names must match exactly.

####Pitching Data

/api/pitchers?firstname=FIRST_NAME&lastname=LAST_NAME (EX. /api/pitchers?firstname=jon&lastname=lester)

Sample Response Structure

{"yearRecords":
  {"2006":{"teamID":"BOS","G":"15","W":"7","L":"2","IP":"81.3","GS":"15","SO":"60","BB":"43","CG":"0","SHO":"0","SV":"0","ERA":"4.76","BAOpp":"0.294","Kper9":"6.64","BBper9":"4.76"},
  "2007":{"teamID":"BOS","G":"12","W":"4","L":"0","IP":"63.0","GS":"11","SO":"50","BB":"31","CG":"0","SHO":"0","SV":"0","ERA":"4.57","BAOpp":"0.257","Kper9":"7.14","BBper9":"4.43"},
  "2008":{"teamID":"BOS","G":"33","W":"16","L":"6","IP":"210.3","GS":"33","SO":"152","BB":"66","CG":"2","SHO":"2","SV":"0","ERA":"3.21","BAOpp":"0.256","Kper9":"6.50","BBper9":"2.82"},...},"nameLast":"Lester","nameFirst":"Jon"}
