# 修改部分

語言翻譯: 繁體中文

用電量Log位置更改: D:\Program Files (x86)\TEMP\powerlog

啟動時默認填入限制功率125W

關閉時自動恢復默認功率且無警告視窗(原本設計是跳出警告視窗)



# VRChat向け GPU電力制限ツール

VRC向け GPU電力制限ツール

![image](https://user-images.githubusercontent.com/66125537/180787763-2033c50c-ad34-4ae4-b42a-95374fa401cd.png)

## 最新リリース(Ver 2.2.1)

[Download (GitHub)](https://github.com/njm2360/VRChatGPUTool/releases/latest)

[Download (BOOTH)](https://njm2360.booth.pm/items/3993173)


## 使用法（インストーラー版）

1. `VRCGPUTool_installer.exe`を実行
   + 管理者権限が必要なためUACが出ますが**はい**を選択して実行してください。
   
1. インストール
   + 画面の指示に従ってインストールします。完了するとスタートメニューとデスクトップにショートカットが作成されます。
   
1. アプリの起動
   + デスクトップ、またはスタートメニュー内のVRChatGPUToolを開きます
  
1. 電力制限値を設定します
   + 設定可能な電力制限値はグラフィックボードによって異なります。設定不可能な値を入力した際にはメッセージが表示されます。
   + 実際に制限できる消費電力は環境により異なります、ボード全体では設定した消費電力より大きくなる場合があります。

1. 制限をする時間を設定します
   + ここでは制限を開始したい時刻と制限を終了する時刻を設定します。

1. 楽しいVRChatライフを
   + 指定した開始時間になると電力制限が働きます。また指定した終了時間になるとリミットが解除されます。
  
※強制的に制限を終了する方法
   + **電力制限解除**をクリックするとデフォルト値になります。
   
### 電気代設定の方法

電気代設定の画面を開くには電力使用履歴から電気代設定をクリックします。  
左側は設定を視覚的に表した円グラフで４つの数字が時間の文字盤を表しています。  
右側は実際に値を設定する欄で現在設定中の値がリストに表示されます。  

時間帯別料金ではない方は0時のままで単価を入力してそのまま追加をクリックしてください。  
上のリストに0時～（設定単価）円という項目が追加されたら適用をクリックして完了です。  
間違えた値を入力してしまった際は上のリストから消したいものを選択して右クリックすると消去できます  
（時間帯別料金ではない場合2つ以上追加しないでください）  

時間帯別料金の方は最初に0時地点の単価を入れて追加をクリックします。  
そのあとは時刻を電気代の単価が変わる時間にして、その時刻から適用される単価を入力して追加します。  
これを必要な分繰り返します。ただし同じ時間で登録したりすることはできません。  
また入力順は自由です（時刻によって自動でソートされます）  
間違えた値を入力してしまった際は上のリストから消したいものを選択して右クリックすると消去できます  

参考として0時～7時が13円、7時～10時が22円、10時～18時が28円、18時～23時が22円、23時～0時が13円の場合の設定例を示します  

![image](https://user-images.githubusercontent.com/66125537/182019494-103fe31c-04fd-4e4a-8658-041ef9400d96.png)
   
### 新機能

1. スタートアップ登録機能
   + 設定よりスタートアップに登録する機能を追加しました。（ユーザーのスタートアップに登録されます）

1. 制限解除時のリミット挙動設定
   + 従来は制限解除時にGPUのデフォルトのリミットに戻される仕様でしたが、デフォルトの値に戻すか指定の値に戻すかをユーザーが選択できるようになりました

1. 電力使用履歴の閲覧機能
   + GPUの消費電力をモニタリングしているため、１時間ごと、１日ごとでの電力使用量を分析することができます
   + 【New】v2.1.0より電気代設定（時間帯別料金も可）に対応したため電気料金も分析できます(v2.2.0より小数にも対応)
   + CSVエクスポート機能によりデータを出力可能です
![image](https://user-images.githubusercontent.com/66125537/181913629-93d09f40-1d35-4330-9ed8-ba9f5451bd30.png)

1. フィードバック機能の搭載
   + バグ報告、新機能の要望などを送れる専用フォームを追加しました。4MBまでの画像ファイルのアップロードに対応しています

### ベータ機能

1. 寝落ち検出機能

時間で設定しようとしても「何時に設定したらいいかわからない」、「いつ寝落ちするかわからない」という人のために**自動検出機能**をベータ機能として提供しています。  
使用したい場合は**自動検出　ベータ版**にチェックを入れます。  

このモードでは寝落ちした際のGPU使用率が比較的安定する特性を用いて電力制限を自動で行います。そのため**GPU使用率が100%に張り付く**ような環境では正しく検出することができません。
~~RTX3090Tiを買いましょう~~　　

この際のしきい値設定は20％を推奨します。しきい値は直近５分間で何％のGPU使用率の変動があったかを計算してしきい値未満の場合に電力制限をかけるというものです。  

大きくしすぎると起きている時でも電力制限がかかったり、逆に小さくしすぎると寝ていても電力制限がかかりません。ここは実際に使いながら値を調節してください。

2. コアクロック制限機能

コアクロック制限をすることでさらなる電力制限をかけることができます。使用する場合はベータ機能の中の**コアクロック制限**にチェックを入れて制限したいクロックを入力してください。  
※ここを利用してオーバークロックをする設定にすることも可能ですが、その場合は自己責任でお願いします。（フォームとしては2000MHzまで入力可能です）
※ここを制限するとSteamVRすらまともに動作しなくなる場合がありますので制限のしすぎに注意してください。上級者向けの設定となります。
