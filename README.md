Identity Server Local
=====

## What's the purpose?

If you are developing a microservice that has authenticated endpoints and if you are running integration tests against this end point, if you would have felt a pain.

If you are not, this is the time to close this window and move on.

The pain, being referred here is the need to get a token or disable auth for integration tests. Options generally used are,

1. Get a real auth token from real auth server for integration test
2. Disable authorization for integration test and or development environment.

While 1 is super painful and might make it hard to do CI/CD process, option 2 is clearly a bad practice and very risky.

To overcome these issues, the traditional general best practices are,

* Mock the auth server using application code to unit and integration tests
* Run the auth server as part of the stack

They both have their own limitations,

Limitation with mocking the auth server using application code is, the need to write that application code. Chances of testing the correct boundaries or strain in the layer when to use mock vs not mock are all practical side affects of this system.

## What is the solution?
Running the auth server locally along with your application in the same stack. Except, make it easy.

## How does this makes 'running the auth server locally' easy?

Follow the trail below to get the answer.


#### How to run auth server locally?

`docker run pageup/identityserver-local`

That`s it!

#### How to configure the auth server with test credentials

1. Create a docker file with the following content
```
FROM pageup/identityserver-local
```
2. Create `Data\auth-data.json` file at the same level as the docker file, with content similar to,
```
{
	"apiResources": [
		{
			"name": "resource1",
	  	"displayName": "resource 1"
		},
		{
			"name": "resource 2",
	  	"displayName": "resource 2"
		},
	],
	"clients" : [
		{
			"clientId" : "test-client-id",
			"allowedGrantType": "ClientCredentials",
			"clientSecrets": [ "secret" ],
			"allowedScopes": ["scope1", "scop2"],
			"claims": [ { "name": "claim1", "value": "calim1value" } ]
		}
	]
}
```

#### How does the above configuration work?

It uses docker's ONBUILD trigger instruction, [docker reference](https://docs.docker.com/engine/reference/builder/#onbuild).

Refer to [dockerfile](https://github.com/PageUpPeopleOrg/IdentityServerLocal/blob/master/Dockerfile#L8) on how it is being used to understand better.

### Sample of how to set it all up

Assuming we are working on an Web API and we are intending to test Web API. The docker-compose file for local and test environment will look like below.

**Example**

docker-compose.yml
```
version: "3"
services:
  api:
    build: ./api      
    ports:
      - 4000
    depends_on:
      - auth

  auth:
    build: ./authlocal
      dockerfile: authlocal-docker
    ports:
      - 4050
```

By doing the above configuration, the API container will have an environment variable called `auth` set on it with value `http://auth:4050`, which is now the auth server you can use.

Any test can then be run after `docker-compose up -d` in the technology of your choice.

### Questions, suggestions, opinions
Please discuss it in issues and tag @humblelistener / @terencet with it.
