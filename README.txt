
①git windows をインストール
②git bash が入ったら。任意のフォルダを作り、右クリック、git bashを選択。
③git init コマンドでgitのローカルリポジトリを作る。
④git remote add origin ttps://ghp_m3psJeOH0BdgzfwdYksFlACYYMIFSWM1ae9AWi@github.com/CGengineer/NovelGame.git
⑤フォルダに何かしらの変更分を用意する。例えば、txtのﾌｧｲﾙを作成する。
⑥git add .
⑦git commit -m "コメントなんでも"
⑧git push origin master （masterは右上のブランチ名）
⑨トークンで入場

何をするか。対策方法
①git hubのアカウントを作成しているかどうか。
②gitのメールアドレスとユーザー名の設定をしているかどうか。
③gitのコラボレーターの権限をつかしているかどうか。
④リポジトリの作成者に、トークンを生成してもらっているかどうか
⑤git remote add origin サーバー名にしているかどうか
⑥コラボレーターに追加した場合、アカウントのメールから認証をしてもらう。

効率化
ブランチをmergeしたら削除できるようにする。
メインブランチをプッシュできないようにする。
ローカルのmasterブランチ以外すべて削除
git checkout master
git branch | xargs git branch -D


変更履歴が違ってプルリクエストが出せない場合
メインブランチに戻って
git pull origin master --allow-unrelated-histories
a
ijyoua
tooookn

ghp_m3psJeOH0BdgzfwdYksFlACYYMIFSWM1ae9AWi


画像を圧縮する場合
https://imguma.com/

文字コードをIFに一括返還する場合
FCChecker.1.2.0