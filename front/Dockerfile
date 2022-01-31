FROM node AS build

ARG REACT_APP_API_ROOT_URL
ENV REACT_APP_API_ROOT_URL $REACT_APP_API_ROOT_URL

WORKDIR /app

COPY package.json .
COPY yarn.lock .
RUN yarn install 

COPY . .
RUN yarn build

FROM nginx AS final

WORKDIR /usr/share/nginx/html
EXPOSE 80
EXPOSE 443

# Remove default nginx static resources
RUN rm -rf ./*

COPY --from=build /app/build .

ENTRYPOINT ["nginx", "-g", "daemon off;"]
