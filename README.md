# Signup
```sh
curl --location 'localhost:5188/api/iam/registro' \
--header 'Content-Type: application/json' \
--data-raw '{
    "nombre": "Facundo",
    "email": "facuserra2002@gmail.com",
    "password": "1234"
}'
```

# Login
## Request
```sh
curl --location 'localhost:5188/api/iam/login' \
--header 'Content-Type: application/json' \
--data-raw '{
    "email": "facuserra2002@gmail.com",
    "password": "1234"
}'
```
## Response
```json
{
    "token": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJmYWN1c2VycmEyMDAyQGdtYWlsLmNvbSIsImV4cCI6MTc1MTc3MjYxMH0.OCQJucE2sbWz0UCnOs3mKeZVawgGwx693h-v5747F6w",
    "refreshToken": "lEXH90yhIaYIz6Tr2sXkpETu3bC30rpclgs1ZTCMPOE="
}
```

# Login
## Refresh
```sh
curl --location 'localhost:5188/api/iam/refresh' \
--header 'Content-Type: application/json' \
--data '{
    "Refresh": "lEXH90yhIaYIz6Tr2sXkpETu3bC30rpclgs1ZTCMPOE="
}'
```
## Response
```json
{
    "token": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJmYWN1c2VycmEyMDAyQGdtYWlsLmNvbSIsImV4cCI6MTc1MTc3MjY2NH0.WQvYDRH86GCogArr7tL3kV7gg1oY9aJdJLxPFiUnTKE",
    "refreshToken": "f8decejMrUwZTG0uK+KY4lnE9pz9NKffuVQiYhNGcpw="
}
```

# Create Book
## Request
```sh
curl --location 'localhost:5188/api/books' \
--header 'Content-Type: application/json' \
--header 'Authorization: Bearer {{access_token}}' \
--data '{
    "nombre": "Prueba libro 2",
    "descripcion": "Prueba de crear libro con un user"
}'
```
## Response
```json
{
    "id": 4,
    "name": "Prueba libro 2",
    "description": "Prueba de crear libro con un user",
    "userId": 2,
    "user": null
}
```

# List Books
## Request
```sh
curl --location --request GET 'localhost:5188/api/books' \
--header 'Content-Type: application/json' \
--header 'Authorization: Bearer {{access_token}}' \
--data '{
    "nombre": "Prueba libro",
    "descripcion": "Prueba de crear libro con un user"
}'
```
## Response
```json
[
    {
        "id": 3,
        "name": "Prueba libro 2",
        "description": "Prueba de crear libro con un user",
        "userId": 2,
        "user": null
    }
]
```

# Delete Book
## Request
```
curl --location --request DELETE 'localhost:5188/api/books/:id' \
--header 'Content-Type: application/json' \
--header 'Authorization: Bearer {{access_token}}' \
```

# Update Book
## Request
```sh
curl --location --request PUT 'localhost:5188/api/books/:id' \
--header 'Content-Type: application/json' \
--header 'Authorization: Bearer {{access_token}}' \
--data '{
    "nombre": "Prueba libro 2",
    "descripcion": "Prueba de crear libro con un user"
}'
```


