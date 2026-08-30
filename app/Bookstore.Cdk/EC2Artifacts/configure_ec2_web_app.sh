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
dnf update -y

# Install .NET 10 runtime from Microsoft package feed
rpm -Uvh https://packages.microsoft.com/config/fedora/40/packages-microsoft-prod.rpm || true
dnf install -y aspnetcore-runtime-10.0

# Install Apache
dnf install -y httpd
systemctl start httpd
systemctl enable httpd

# Add TLS Support
dnf install -y mod_ssl

# Generate self-signed certificate (make-dummy-cert doesn't exist on AL2023)
openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
  -keyout /etc/pki/tls/private/localhost.key \
  -out /etc/pki/tls/certs/localhost.crt \
  -subj "/C=US/ST=State/L=City/O=BobsBookstore/CN=localhost"

cp $APACHE_SSL_CONFIG_FILE /etc/httpd/conf.d/ssl.conf

# Add Bookstore Admin App Virtual Host Config
cp $VIRTUAL_HOST_CONFIG /etc/httpd/conf.d/bobsbookstore.conf

# Restart Apache
systemctl restart httpd

# Install bookstore admin app into the /var/www/bobsbookstore directory
mkdir -p /var/www/bobsbookstore
cp $SAMPLE_APP /var/www/bobsbookstore/bobsbookstore.zip
cd /var/www/bobsbookstore
unzip bobsbookstore.zip
rm bobsbookstore.zip
usermod -a -G apache ec2-user
chown ec2-user:apache /var/www
chmod 2775 /var/www && find /var/www -type d -exec chmod 2775 {} \;
find /var/www -type f -exec chmod 0664 {} \;

# Install the Kestrel Service 
cp $KESTREL_SERVICE /etc/systemd/system/bobsbookstore.service
systemctl enable bobsbookstore.service
systemctl start bobsbookstore.service
systemctl status bobsbookstore.service
