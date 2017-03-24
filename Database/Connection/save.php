<?php
include "dbconfig.php";
if(isset($_POST['AppKey'])
 && isset($_POST['UserId'])
 && isset($_POST['UserName'])
 && isset($_POST['UserToken'])
 && isset($_POST['UserPosX'])
 && isset($_POST['UserPosY'])
 && isset($_POST['UserPosZ'])
 && isset($_POST['UserLevel'])
 && isset($_POST['UserCurrency'])
 && isset($_POST['UserExpierance'])
 && isset($_POST['UserHealth'])
 && isset($_POST['UserPower'])
 && isset($_POST['UserGpsX'])
 && isset($_POST['UserGpsY'])
 && isset($_POST['UserGpsZ'])
 && isset($_POST['UserRotX'])
 && isset($_POST['UserRotY'])
 && isset($_POST['UserRotZ']))
{

	$AppKey = $_POST['AppKey'];
	$MatchedAppKey = "jZuHiJtYS";

	if($AppKey != $MatchedAppKey)
	{
	
     return;
	}

    $UserId= $_POST['UserId'];
    $UserName= $_POST['UserName'];
    $UserToken= $_POST['UserToken'];
    $UserPosX= $_POST['UserPosX'];
    $UserPosY= $_POST['UserPosY'];
    $UserPosZ= $_POST['UserPosZ'];
    $UserLevel= $_POST['UserLevel'];
    $UserCurrency= $_POST['UserCurrency'];
    $UserExpierance= $_POST['UserExpierance'];
    $UserHealth= $_POST['UserHealth'];
    $UserPower= $_POST['UserPower'];
    $UserGpsX= $_POST['UserGpsX'];
    $UserGpsY= $_POST['UserGpsY'];
    $UserGpsZ= $_POST['UserGpsZ'];
    $UserRotX= $_POST['UserRotX'];
    $UserRotY= $_POST['UserRotY'];
    $UserRotZ= $_POST['UserRotZ'];

	
				
				$sql = "SELECT * FROM basic_players_info WHERE  UserId = '$UserId' LIMIT 1"; 
  $query = mysqli_query($conn,$sql) or trigger_error("Query Failed: " . mysqli_error()); 
 
  
  if (mysqli_num_rows($query) > 0) 
  { 
  
  
	  $sql = "UPDATE basic_players_info 
    SET UserPosX = '$UserPosX',
     UserPosY='$UserPosY', 
     UserPosZ='$UserPosZ', 
     UserLevel='$UserLevel',
     UserCurrency='$UserCurrency', 
     UserExpierance='$UserExpierance', 
     UserHealth='$UserHealth', 
     UserPower='$UserPower',
     UserGpsX='$UserGpsX',
     UserGpsY='$UserGpsY',
     UserGpsZ='$UserGpsZ',
     UserRotX='$UserRotX',
     UserRotY='$UserRotY',
     UserRotZ='$UserRotZ'
     WHERE UserId = '$UserId'" ;
      $res = mysqli_query( $conn,$sql );
      
	  
	 
	   
	 
  }
  

				
		return;
}
else
{
	echo 0;
	return;
}





			

?>