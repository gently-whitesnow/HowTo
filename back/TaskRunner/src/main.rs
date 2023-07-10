use hyper::service::{make_service_fn, service_fn};
use hyper::{Body, Method, Request, Response, Server};
use std::convert::Infallible;
use std::net::SocketAddr;
use task_runner::response::{not_allowed, not_found, not_implemented};

async fn main_handler(req: Request<Body>) -> Result<Response<Body>, Infallible> {
    let path = req.uri().path();
    let query = req.uri().query().unwrap_or_default();

    match req.method() {
        &Method::POST => match path {
            "/register" => Ok(not_implemented()),
            "/test" => Ok(not_implemented()),
            _ => Ok(not_found()),
        },
        _ => Ok(not_allowed()),
    }
}

#[tokio::main]
async fn main() {
    let addr = SocketAddr::from(([0, 0, 0, 0], 1337));

    let make_svc = make_service_fn(|_conn| async { Ok::<_, Infallible>(service_fn(main_handler)) });

    let server = Server::bind(&addr).serve(make_svc);

    if let Err(e) = server.await {
        eprintln!("server error: {}", e);
    }
}
