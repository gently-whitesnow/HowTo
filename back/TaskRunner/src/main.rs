use hyper::body::to_bytes;
use hyper::service::{make_service_fn, service_fn};
use hyper::{Body, Method, Request, Response, Server};
use std::convert::Infallible;
use std::net::SocketAddr;
use task_runner::language::Language;
use task_runner::query::parse_query;
use task_runner::register_test::register_test_handler;
use task_runner::response::{not_allowed, not_found, not_implemented};

async fn available_handler() -> Result<Response<Body>, Infallible> {
    Ok(Response::builder()
        .body(Language::to_json().to_string().into())
        .unwrap())
}

async fn make_string_body(req: &mut Request<Body>) -> String {
    match to_bytes(req.body_mut()).await {
        Ok(bytes) => match String::from_utf8((&bytes).to_vec()) {
            Ok(str) => str,
            _ => String::default(),
        },
        _ => String::default(),
    }
}

async fn main_handler(mut req: Request<Body>) -> Result<Response<Body>, Infallible> {
    let path = req.uri().path();
    let query = parse_query(req.uri().query().unwrap_or_default());

    match req.method() {
        &Method::POST => match path {
            "/register_test" => {
                register_test_handler(query, make_string_body(&mut req).await).await
            }
            "/register_solution" => Ok(not_implemented()),
            "/available" => available_handler().await,
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
