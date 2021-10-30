# What is this?
This is my "Dunkelmann-API", which can also be accessed
under [api.dunkelmann.eu](https://api.dunkelmann.eu).

# How to use?
If you have `nginx`, you can use the following sample config:

```nginx
server {
    listen 443 ssl;
    
    ssl_certificate /etc/cert/fullchain.pem;
    ssl_certificate_key /etc/cert/privkey.pem;

    server_name api.dunkelmann.eu;

    location / {
        proxy_pass http://localhost:8888/;
    }
}

```
