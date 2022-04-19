<?php
  if($_FILES['imageData']['name'])
  {
    $file = $_FILES['imageData']['tmp_name'];

    $id = uniqid();
    
    $save_path="/var/www/html/api/data/uploads/";
    
    if (!file_exists($save_path)) {
      mkdir($save_path, 0777, true);
    }

    $fileName = $id . ".png";

    move_uploaded_file($file, $save_path . $fileName);

    echo $id;
  }
?>
