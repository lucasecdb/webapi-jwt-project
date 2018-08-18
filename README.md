# JWT with WebApi error sample project

To start the project in the docker container, run the following commands

```bash
docker build -t jwt-sample-project .
docker run -it --rm -p 5000:80 --name jwt-sample-project-container jwt-sample-project
```

And to reproduce the error in the API, just send a POST request to `localhost:5000/api/auth/login`,
then check your terminal to see the stack trace and error message.

