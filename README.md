# EDC 2021 - Radix Client Certificate Authentication

Demonstrate how to configure client certificate authentication in Radix

## Create self signed CA and certificate

https://campus.barracuda.com/product/webapplicationfirewall/doc/12193120/creating-a-client-certificate/

### Generate CA key and certificate
openssl genrsa 2048 > selfsigned-ca-key.pem
openssl req -new -x509 -nodes -days 1000 -key selfsigned-ca-key.pem > selfsigned-ca-cert.pem

### Generate client certificate key and certificate
openssl req -newkey rsa:2048 -days 1000 -nodes -keyout selfsigned-nils-key.pem > selfsigned-nils-req.pem
openssl x509 -req -in selfsigned-nils-req.pem -days 1000 -CA selfsigned-ca-cert.pem -CAkey selfsigned-ca-key.pem -set_serial 01 > selfsigned-nils-cert.pem

openssl req -newkey rsa:2048 -days 1000 -nodes -keyout selfsigned-sergey-key.pem > selfsigned-sergey-req.pem
openssl x509 -req -in selfsigned-sergey-req.pem -days 1000 -CA selfsigned-ca-cert.pem -CAkey selfsigned-ca-key.pem -set_serial 01 > selfsigned-sergey-cert.pem

## Call API with client certificate

### With no client certificate

curl --request GET --url https://api-edc2021-radix-client-cert-off.playground.radix.equinor.com/data
curl --request GET --url https://api-edc2021-radix-client-cert-optional-no-ca.playground.radix.equinor.com/data
curl --request GET --url https://api-edc2021-radix-client-cert-optional.playground.radix.equinor.com/data
curl --request GET --url https://api-edc2021-radix-client-cert-on.playground.radix.equinor.com/data

### As Nils
curl --request GET --url https://api-edc2021-radix-client-cert-off.playground.radix.equinor.com/data --cert selfsigned-nils-cert.pem --key selfsigned-nils-key.pem

curl --request GET --url https://api-edc2021-radix-client-cert-optional-no-ca.playground.radix.equinor.com/data --cert selfsigned-nils-cert.pem --key selfsigned-nils-key.pem

curl --request GET --url https://api-edc2021-radix-client-cert-optional.playground.radix.equinor.com/data --cert selfsigned-nils-cert.pem --key selfsigned-nils-key.pem

curl --request GET --url https://api-edc2021-radix-client-cert-on.playground.radix.equinor.com/data --cert selfsigned-nils-cert.pem --key selfsigned-nils-key.pem

### As Sergey

curl --request GET --url https://api-edc2021-radix-client-cert-optional-no-ca.playground.radix.equinor.com/data --cert selfsigned-sergey-cert.pem --key selfsigned-sergey-key.pem

curl --request GET --url https://api-edc2021-radix-client-cert-optional.playground.radix.equinor.com/data --cert selfsigned-sergey-cert.pem --key selfsigned-sergey-key.pem

curl --request GET --url https://api-edc2021-radix-client-cert-on.playground.radix.equinor.com/data --cert selfsigned-sergey-cert.pem --key selfsigned-sergey-key.pem

### Call API via APIM

curl --request GET --url https://api-test.gateway.equinor.com/edc-radix-cert-nst/off/data

curl --request GET --url https://api-test.gateway.equinor.com/edc-radix-cert-nst/optional-no-ca/data

curl --request GET --url https://api-test.gateway.equinor.com/edc-radix-cert-nst/optional/data 

curl --request GET --url https://api-test.gateway.equinor.com/edc-radix-cert-nst/on/data
