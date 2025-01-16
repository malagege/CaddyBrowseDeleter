FROM arm32v6/caddy:latest

RUN xcaddy build --with \
    github.com/pyed/ipfilter

# 將自定義 Caddyfile 和插件映射到容器中
#COPY Caddyfile /etc/caddy/Caddyfile
#COPY plugins/ /usr/share/caddy/