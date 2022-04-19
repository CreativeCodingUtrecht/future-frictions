# FROM nginx:1.19.3-alpine
# COPY . /usr/share/nginx/html
# EXPOSE 80
# ENTRYPOINT [ "nginx", "-g", "daemon off;" ]

FROM php:apache

RUN docker-php-ext-install mysqli

WORKDIR /var/www/html

COPY . .

RUN chown -R www-data:www-data /var/www/html

COPY uploads.ini /usr/local/etc/php/conf.d/uploads.ini

EXPOSE 80

# docker build -t registry.gitlab.com/katpatat/future-frictions:dev .

# docker push registry.gitlab.com/katpatat/future-frictions:dev