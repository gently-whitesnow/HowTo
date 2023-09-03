use std::{collections::HashMap, str::FromStr};

pub type QueryMap = HashMap<String, String>;

fn query_key_value(param: &str) -> (&str, &str) {
    let mut iter = param.splitn(2, "=");
    (
        iter.next().unwrap_or_default(),
        iter.next().unwrap_or_default(),
    )
}

pub fn parse_query(query: &str) -> QueryMap {
    let mut parsed = HashMap::new();
    for param in query.split("&") {
        let (key, value) = query_key_value(param).clone();
        parsed.insert(String::from_str(key).unwrap_or_default(), String::from_str(value).unwrap_or_default());
    }
    parsed
}

pub fn get_query_bool(value: &str) -> Option<bool> {
    match value {
        "1" | "yes" | "true" | "da" => Some(true),
        "0" | "no" | "false" | "net" => Some(false),
        _ => None,
    }
}
