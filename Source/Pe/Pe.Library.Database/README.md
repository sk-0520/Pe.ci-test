# Pe.Library.Database

使用者側は自分で明示的なトランザクションを発行するだけでやってることは [Dapper](https://github.com/DapperLib/Dapper) ラッパー。  
※でも Dapper 自体は表に出さない思想。

基本的に根っ子でトランザクション開始してコンテキストを引き渡しながら使う感じ。
