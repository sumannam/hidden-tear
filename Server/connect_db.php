<?php
	//change you connection options
	$con=mysqli_connect("localhost","root","techfin", "hidden_tear");	 
        
    if (!$con){
     die ("Colud not connect to server: ".mysql_error());
    }
?>