CREATE DATABASE `basic-test`;
CREATE USER 'basic-test'@'%' IDENTIFIED BY 'basic-test';
GRANT ALL PRIVILEGES ON `basic-test`.* TO 'basic-test'@'%';
FLUSH PRIVILEGES;
