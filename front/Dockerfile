FROM node AS build

WORKDIR /app

RUN corepack enable
RUN corepack prepare yarn@stable --activate

COPY . .
RUN yarn build

FROM nginx AS final

WORKDIR /usr/share/nginx/html
EXPOSE 80
EXPOSE 443

# Remove default nginx static resources
RUN rm -rf ./*

COPY --from=build /app/build .
COPY ./nginx.ssh-activate.sh /docker-entrypoint.d/15-ssh-activate.sh
COPY ./nginx.http.conf.template /etc/nginx/templates/http.conf.template
COPY ./nginx.https.conf.template /etc/nginx/templates/https.conf.template

# ENTRYPOINT ["/docker-entrypoint.sh"]
CMD ["nginx", "-g", "daemon off;"]
