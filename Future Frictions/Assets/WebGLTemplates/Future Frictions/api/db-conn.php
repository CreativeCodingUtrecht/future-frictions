<?php 
  $servername = "130.89.1.124:30336";
  $dbname = "db";
  $username = "future-frictions";
  $password = "53qCA@aphFU&";

  $mysqli = new mysqli($servername, $username, $password, $dbname);

  if ($mysqli -> connect_errno) {
    echo "Failed to connect to MySQL: " . $mysqli -> connect_error;
    exit();
  }
?>