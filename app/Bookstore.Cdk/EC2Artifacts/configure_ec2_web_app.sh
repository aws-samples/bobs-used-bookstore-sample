#!/bin/bash -xe
exec > >(tee /var/log/user-data.log|logger -t user-data -s 2>/dev/console) 2>&1

# Read the first parameter into $SAMPLE_APP
if [[ "$1" != "" ]]; then
    SAMPLE_APP="$1"
else
    echo "Please specify the location of web application you are trying to deploy."
    exit 1
fi

# Read the second parameter into $APACHE_SSL_CONFIG_FILE
if [[ "$2" != "" ]]; then
    APACHE_SSL_CONFIG_FILE="$2"
else
    echo "Please specify the location of the Apache ssl.conf file."
    exit 1
fi

# Read the third parameter into $VIRTUAL_HOST_CONFIG
if [[ "$3" != "" ]]; then
    VIRTUAL_HOST_CONFIG="$3"
else
    echo "Please specify the location of the admin app virtual host config file."
    exit 1
fi

# Read the fourth parameter into $KESTREL_SERVICE
if [[ "$4" != "" ]]; then
    KESTREL_SERVICE="$4"
else
    echo "Please specify the location of the Kestrel service file."
    exit 1
fi


# Install latest updates
yum update -y

# Install Apache
sudo yum install -y httpd
sudo systemctl start httpd
sudo systemctl enable httpd

# Add TLS Support
sudo yum install -y mod_ssl
cd /etc/pki/tls/certs
sudo ./make-dummy-cert localhost.crt
cp $APACHE_SSL_CONFIG_FILE /etc/httpd/conf.d/ssl.conf

# Add Bookstore Admin App Virtual Host Config
cp $VIRTUAL_HOST_CONFIG /etc/httpd/conf.d/bobsbookstore.conf

# Restart Apache
sudo systemctl restart httpd

# Install bookstore admin app into the /var/www/bobsbookstore directory
mkdir -p /var/www/bobsbookstore
cp $SAMPLE_APP /var/www/bobsbookstore/bobsbookstore.zip
cd /var/www/bobsbookstore
unzip bobsbookstore.zip
rm bobsbookstore.zip
sudo usermod -a -G apache ec2-user
sudo chown ec2-user:apache /var/www
sudo chmod 2775 /var/www && find /var/www -type d -exec sudo chmod 2775 {} \;
find /var/www -type f -exec sudo chmod 0664 {} \;

# Install the Kestrel Service 
cp $KESTREL_SERVICE /etc/systemd/system/bobsbookstore.service
sudo systemctl enable bobsbookstore.service
sudo systemctl start bobsbookstore.service
sudo systemctl status bobsbookstore.service