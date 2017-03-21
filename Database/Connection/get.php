<?php
include "dbconfig.php";
if(isset($_GET['UserId']))
{
				//set the vars of the get value passed along.
	$UserId= $_GET['UserId'];
				
   $sql = "SELECT * FROM basic_players_info WHERE  UserId = '$UserId' LIMIT 1"; 
  $query = mysqli_query($conn,$sql) or trigger_error("Query Failed: " . mysqli_error()); 
 
  
  if (mysqli_num_rows($query) > 0) 
  { 
  // the user is already registered fetch the data
   $row = mysqli_fetch_assoc($query);
  
   $Id = $row['Id'];
   $UserId = $row['UserId'];
   $UserName = $row['UserName'];
   $UserPic = $row['UserPic'];
   $UserToken = $row['UserToken'];
   $UserPosX = $row['UserPosX'];
   $UserPosY = $row['UserPosY'];
   $UserPosZ = $row['UserPosZ'];
   $UserLevel = $row['UserLevel'];
   $UserCurrency = $row['UserCurrency'];
   $UserExpierance = $row['UserExpierance'];
   $UserHealth = $row['UserHealth'];
   $UserPower = $row['UserPower'];
   $UserGpsX = $row['UserGpsX'];
   $UserGpsY = $row['UserGpsY'];
   $UserGpsZ = $row['UserGpsZ'];
   $UserVungleApi = $row['UserVungleApi'];
   $UserAdcolonyApi = $row['UserAdcolonyApi'];
   $UserAdcolonyZone = $row['UserAdcolonyZone'];
   $UserRotX = $row['UserRotX'];
   $UserRotY = $row['UserRotY'];
   $UserRotZ = $row['UserRotZ'];
  
   
   echo "|Id|".$Id.
   "|UserId|".$UserId.
   "|UserName|".$UserName.
   "|UserPic|".$UserPic.
   "|UserToken|".$UserToken.
   "|UserPosX|".$UserPosX.
   "|UserPosY|".$UserPosY.
   "|UserPosZ|".$UserPosZ.
   "|UserLevel|".$UserLevel.
   "|UserCurrency|".$UserCurrency.
   "|UserExpierance|".$UserExpierance.
   "|UserHealth|".$UserHealth.
   "|UserPower|".$UserPower.
   "|UserGpsX|".$UserGpsX.
   "|UserGpsY|".$UserGpsY.
   "|UserGpsZ|".$UserGpsZ.
   "|UserVungleApi|".$UserVungleApi.
   "|UserAdcolonyApi|".$UserAdcolonyApi.
   "|UserAdcolonyZone|".$UserAdcolonyZone.
   "|UserRotX|".$UserRotX.
   "|UserRotY|".$UserRotY.
   "|UserRotZ|".$UserRotZ;
   
  }
  else
  {
	  $error = "ErrorNoUserFound";
	  echo "|UserId|".$UserId."|Error|".$error;
  }
  return;
}
else
{
	echo 0;
	return;
}
			
?>