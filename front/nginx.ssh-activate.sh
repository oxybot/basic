#!/bin/bash

if [[ -z ${NGINX_SSL_CERTIFICATE} ]] || [[ -z ${NGINX_SSL_CERTIFICATE_KEY} ]]
then
    echo -e "SSL is disabled. NGINX_SSL_CERTIFICATE and NGINX_SSL_CERTIFICATE_KEY should be defined for SSL to be enabled"
    if [[ -f "/etc/nginx/templates/https.conf.template" ]]
    then
        mv /etc/nginx/templates/https.conf.template /etc/nginx/templates/https.conf.tpl
    fi
else
    if [[ -f "/etc/nginx/templates/https.conf.tpl" ]]
    then
        mv /etc/nginx/templates/https.conf.tpl /etc/nginx/templates/https.conf.template
    fi
fi
