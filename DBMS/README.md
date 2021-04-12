# Configuration

* Project does not include MongoDB server. MongoDB must be running elsewhere
without authentication. MongoDB host can be configured in `Web.config`.
* The server host should be static, as it is stored in the project file.
If not, it can be configured in the project Properties > Web > Servers. If it is
static, it should be hosted at `https://localhost:44322`.
* The client sets the server address in `Form1.cs`. If the server host is
static, this shouldn't *have* to be changed.


# Acknowledgements

* Most code used for JWT authentication used from [`WebApi.Jwt`](https://github.com/cuongle/WebApi.Jwt/) on Github.

