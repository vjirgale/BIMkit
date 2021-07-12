# Deployment Documentation

## For Development
The following will guide you through setting up the project for local development.

### Required Software
* yarn
* nodejs
* python3
* git
* virtualenv

Yarn can be installed [here](https://classic.yarnpkg.com/en/docs/install/#debian-stable). Ensure the latest stable version of Node.js (at time of writing, 14.15.1) is installed using one of the methods found [here](https://phoenixnap.com/kb/update-node-js-version). Python3, git, and virtualenv can be installed via apt-get.

### Installation Process
1. Clone the repository to the desired folder on your development device. Navigate to the root directory of the repository. 

2. Setup the frontend environment
    1. Navigate to `/frontend/natural-language-rules`.
    2. To install all Node packages required, run `yarn install`.

3. Setup the backend environment
    1. Navigate to the root of the repo, and then `/backend`.
    2. Setup a virtual environment with the command `virtualenv venv --python=python3`. Activate it with `source backend/venv/bin/activate` when you are 
developing, and deactivate it with `deactivate` when not.
    3. Install the required Python packages with `pip install -r ../requirements.txt`

### Development Environment Startup
1. Navigate to the root directory of the repo. Activate your Python virtual environment with `source /backend/venv/bin/activate`.

2. Open two terminals. In the first, navigate to `/frontend/natural-language-rules`. In the second, navigate to `/backend`. The first will be running the frontend development server, and the second will be running the backend development server.

3. In the frontend terminal, run `yarn start`. The React dev server should start and open a tab in your default browser. If not, open a new browser tab and navigate to  `http://localhost:3000/`

4. In the backend terminal, run `flask run`. The Flask dev server should start. You are now free to start developing. Any saved files should result in the appropriate server being reloaded.

### Development Environment Shutdown
1. In each terminal window, enter the `Ctrl + C` key combo to stop each respective server.

2. Deactivate the virtual environment with `deactivate`.


***

## For Production
The following will guide you through deploying the application on a remote instance to host and run the site.

The site runs with a combination of a static Nginx server and a Flask-Gunicorn server running on the same machine. Requests are forwarded to the backend API as appropriate, and we will ensure the two are configured properly. This assumes the site is being run on an Ubuntu Linux instance.

### Required Software
* yarn
* nodejs
* python3
* nginx
* git
* virtualenv

Yarn can be installed [here](https://classic.yarnpkg.com/en/docs/install/#debian-stable). Ensure the latest stable version of Node.js (at time of writing, 14.15.1) is installed using one of the methods found [here](https://phoenixnap.com/kb/update-node-js-version). Python3, git, virtualenv and nginx can be installed via apt-get.

_**Before proceeding, on your production machine, follow steps 1-3 of the same installation process used for development.**_

### Frontend Installation Process
1. Navigate to `/frontend/natural-language-rules` and run `yarn build`. This will build an optimized production build of the frontend that we will deploy.

2. Navigate to `/etc/nginx/`. Run `sudo rm sites-enabled/default` to remove the default configuration for the site included with Nginx.

3. Edit the configuration for the project with `sudo nano sites-available/default`. `nano` can be replaced with a similar editing tool if desired. 
Replace the contents of the file with the following, replacing `{**root_folder_here**}` with the root directory of the repository. Note that this configuration uses port 80 and thus uses HTTP vs HTTPS.

```
server {
    listen 80;
    listen [::]:80 default_server;
    root {**root_folder_here**}/frontend/natural-language-rules/build;
    index index.html;

    location / {
        try_files $uri $uri/ =404;
    }

    # This sends forward all requests for /api to port 5000 -- our Flask server
    location /api {
        include proxy_params;
        proxy_pass http://localhost:5000;
    }
    # This similarily forwards all requests for /socket.io to the same Flask server.
    location /socket.io {
      include proxy_params;
      proxy_pass http://localhost:5000;
    }
}
```


4. Create a symlink with our new configuration by running `sudo ln -s /etc/nginx/sites-available/default /etc/nginx/sites-enabled/default`.

5. Reload and restart the Nginx server with `sudo systemctl reload nginx`. The Nginx server will now serve the compiled version of the application.

### Backend Installation process
1. Navigate to `/etc/systemd/system`. We will add a service to run our Flask server.

2. Create a file called `natural-language-rules.service` and enter the following contents, replacing `{**root_folder_here**}` with the root directory of the repository:
```
[Unit]
Description=Flask API for NLR project
After=network.target

[Service]
User=ubuntu
WorkingDirectory={**root_folder_here**}/backend
ExecStart={**root_folder_here**}/backend/venv/bin/gunicorn -k gevent -b 127.0.0.1:5000 server:app

Restart=always

[Install]
WantedBy=multi-user.target

```

3. Run `sudo systemctl daemon-reload` followed by `sudo systemctl start natural-language-rules` to reload and restart the Flask server. It will now run in the background as a service at boot.

4. Reload the frontend with `sudo systemctl reload nginx` to ensure the front and backend are connected appropriately.


### Updating the Server
When the project has been updated, a few commands will need to be run to ensure the server reflects changes made in the repository. From the root directory of the repository, run:


1. `git pull`
2. `backend/venv/bin/pip install -r requirements.txt`
3. `yarn install && yarn build`
4. `sudo systemctl reload nginx`
5. `sudo systemctl restart natural-language-rules`

These can either be run manually one by one, run using a bash script, or run via SSH and GitHub actions if you wish to automate the process on an update to master.