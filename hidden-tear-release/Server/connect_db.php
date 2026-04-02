<?php
	//change you connection options
	$con=mysqli_connect("localhost","malware","security", "smnam_malware");	 
        
    if (!$con){
     die ("Colud not connect to server: ".mysql_error());
    }
?>