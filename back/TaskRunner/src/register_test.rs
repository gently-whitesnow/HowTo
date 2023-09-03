use crate::{
    language::Language,
    query::QueryMap,
    response::{bad_request, not_implemented},
};
use hyper::{Body, Response};
use std::{convert::Infallible, path::PathBuf};

fn perform_index() -> u32 {
    
}

pub async fn register_test_handler(
    query: QueryMap,
    body: String,
) -> Result<Response<Body>, Infallible> {
    if !query.contains_key("lang") {
        return Ok(bad_request("expected query 'lang'"));
    }
    let lang_num = match u32::from_str_radix(query["lang"].clone().as_str(), 10) {
        Ok(num) => num,
        _ => u32::MAX,
    };
    let lang = match Language::from_u32(lang_num) {
        Ok(language) => language,
        Err(msg) => return Ok(bad_request(msg.as_str()))
    };
    let mut source_path: PathBuf = ["/", "data", "sources"].iter().collect();
    source_path.push(format!("source_{}_{}.{}", lang.to_u32(), ));
    let mut source_file = std::fs::File::create("");
    Ok(not_implemented())
}
