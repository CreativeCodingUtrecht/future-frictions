# FROM nginx:1.19.3-alpine
# COPY . /usr/share/nginx/html
# EXPOSE 80
# ENTRYPOINT [ "nginx", "-g", "daemon off;" ]

FROM php:apache

RUN docker-php-ext-install mysqli

COPY . /var/www/html

EXPOSE 80