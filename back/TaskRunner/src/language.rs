use serde::Serialize;
use std::str::FromStr;
use strum::IntoEnumIterator;
use strum_macros::EnumIter;

#[derive(Copy, Clone, EnumIter)]
pub enum Language {
    Plain = 0,
    Python310 = 1,
    Cpp17Clang14 = 2,
}

impl Language {
    pub fn to_str(&self) -> &str {
        match self {
            Self::Plain => "Plain Text",
            Self::Python310 => "Python 3.10",
            Self::Cpp17Clang14 => "C++17 Clang++14",
        }
    }

    pub fn to_u32(&self) -> u32 {
        *self as u32
    }

    pub fn to_json() -> serde_json::Value {
        let mut languages = std::vec::Vec::new();
        for lang in Language::iter() {
            languages.push(LanguageDescription {
                id: lang.to_u32(),
                name: String::from_str(lang.to_str()).unwrap(),
            });
        }
        serde_json::json!(languages)
    }

    pub fn from_u32(num: u32) -> Result<Language, String> {
        match num {
            0 => Ok(Self::Plain),
            1 => Ok(Self::Python310),
            2 => Ok(Self::Cpp17Clang14),
            _ => Err(String::from_str("Unsupported lang id").unwrap()),
        }
    }

    pub fn extension(&self) -> &str {
        match self {
            Self::Plain => "",
            Self::Python310 => ".py",
            Self::Cpp17Clang14 => ".cpp",
        }
    }
}

#[derive(Serialize)]
struct LanguageDescription {
    id: u32,
    name: String,
}
