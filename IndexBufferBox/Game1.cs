using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace IndexBufferBox
{
	/// <summary>
	/// ゲームメインクラス
	/// </summary>
	public class Game1 : Game
	{
    /// <summary>
    /// グラフィックデバイス管理クラス
    /// </summary>
    private readonly GraphicsDeviceManager _graphics = null;

    /// <summary>
    /// スプライトのバッチ化クラス
    /// </summary>
    private SpriteBatch _spriteBatch = null;

    /// <summary>
    /// 基本エフェクト
    /// </summary>
    private BasicEffect _basicEffect = null;

    /// <summary>
    /// 頂点バッファ
    /// </summary>
    private VertexBuffer _vertexBuffer = null;

    /// <summary>
    /// インデックスバッファ
    /// </summary>
    private IndexBuffer _indexBuffer = null;

    /// <summary>
    /// インデックスバッファの各頂点番号配列
    /// </summary>
    private static readonly Int16[] _vertexIndices = new Int16[] {
            2, 0, 1, // １枚目のポリゴン
            1, 3, 2, // ２枚目のポリゴン
            4, 0, 2, // ３枚目のポリゴン
            2, 6, 4, // ４枚目のポリゴン
            5, 1, 0, // ５枚目のポリゴン
            0, 4, 5, // ６枚目のポリゴン
            7, 3, 1, // ７枚目のポリゴン
            1, 5, 7, // ８枚目のポリゴン
            6, 2, 3, // ９枚目のポリゴン
            3, 7, 6, // １０枚目のポリゴン
            4, 6, 7, // １１枚目のポリゴン
            7, 5, 4  // １２枚目のポリゴン
        };


    /// <summary>
    /// GameMain コンストラクタ
    /// </summary>
    public Game1()
    {
      // グラフィックデバイス管理クラスの作成
      _graphics = new GraphicsDeviceManager(this);

      // ゲームコンテンツのルートディレクトリを設定
      Content.RootDirectory = "Content";

      // マウスカーソルを表示
      IsMouseVisible = true;
    }

    /// <summary>
    /// ゲームが始まる前の初期化処理を行うメソッド
    /// グラフィック以外のデータの読み込み、コンポーネントの初期化を行う
    /// </summary>
    protected override void Initialize()
    {
      // TODO: ここに初期化ロジックを書いてください

      // コンポーネントの初期化などを行います
      base.Initialize();
    }

    /// <summary>
    /// ゲームが始まるときに一回だけ呼ばれ
    /// すべてのゲームコンテンツを読み込みます
    /// </summary>
    protected override void LoadContent()
    {
      // テクスチャーを描画するためのスプライトバッチクラスを作成します
      _spriteBatch = new SpriteBatch(GraphicsDevice);

      // エフェクトを作成
      _basicEffect = new BasicEffect(GraphicsDevice);

      // エフェクトで頂点カラーを有効にする
      _basicEffect.VertexColorEnabled = true;

      // ビューマトリックスをあらかじめ設定 ((10, 10, 10) から原点を見る)
      _basicEffect.View = Matrix.CreateLookAt(
              new Vector3(10.0f, 10.0f, 10.0f),
              Vector3.Zero,
              Vector3.Up
          );

      // プロジェクションマトリックスをあらかじめ設定
      _basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(
              MathHelper.ToRadians(45.0f),
              (float)GraphicsDevice.Viewport.Width /
                  (float)GraphicsDevice.Viewport.Height,
              1.0f,
              100.0f
          );

      // 頂点の数
      int vertexCount = 8;

      // 頂点バッファ作成
      _vertexBuffer = new VertexBuffer(GraphicsDevice,
          typeof(VertexPositionColor), vertexCount, BufferUsage.None);

      // 頂点データを作成する
      VertexPositionColor[] vertives = new VertexPositionColor[vertexCount];

      vertives[0] = new VertexPositionColor(new Vector3(-2.0f, 2.0f, -2.0f), Color.Yellow);
      vertives[1] = new VertexPositionColor(new Vector3(2.0f, 2.0f, -2.0f), Color.Gray);
      vertives[2] = new VertexPositionColor(new Vector3(-2.0f, 2.0f, 2.0f), Color.Purple);
      vertives[3] = new VertexPositionColor(new Vector3(2.0f, 2.0f, 2.0f), Color.Red);
      vertives[4] = new VertexPositionColor(new Vector3(-2.0f, -2.0f, -2.0f), Color.SkyBlue);
      vertives[5] = new VertexPositionColor(new Vector3(2.0f, -2.0f, -2.0f), Color.Orange);
      vertives[6] = new VertexPositionColor(new Vector3(-2.0f, -2.0f, 2.0f), Color.Green);
      vertives[7] = new VertexPositionColor(new Vector3(2.0f, -2.0f, 2.0f), Color.Blue);

      // 頂点データを頂点バッファに書き込む
      _vertexBuffer.SetData(vertives);

      // インデックスバッファを作成
      _indexBuffer = new IndexBuffer(GraphicsDevice,
          IndexElementSize.SixteenBits, 3 * 12, BufferUsage.None);

      // 頂点インデックスを書き込む
      _indexBuffer.SetData(_vertexIndices);
    }

    /// <summary>
    /// ゲームが終了するときに一回だけ呼ばれ
    /// すべてのゲームコンテンツをアンロードします
    /// </summary>
    protected override void UnloadContent()
    {
      // TODO: ContentManager で管理されていないコンテンツを
      //       ここでアンロードしてください
    }

    /// <summary>
    /// 描画以外のデータ更新等の処理を行うメソッド
    /// 主に入力処理、衝突判定などの物理計算、オーディオの再生など
    /// </summary>
    /// <param name="gameTime">このメソッドが呼ばれたときのゲーム時間</param>
    protected override void Update(GameTime gameTime)
    {
      // ゲームパッドの Back ボタン、またはキーボードの Esc キーを押したときにゲームを終了させます
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
      {
        Exit();
      }

      // TODO: ここに更新処理を記述してください

      // 登録された GameComponent を更新する
      base.Update(gameTime);
    }

    /// <summary>
    /// 描画処理を行うメソッド
    /// </summary>
    /// <param name="gameTime">このメソッドが呼ばれたときのゲーム時間</param>
    protected override void Draw(GameTime gameTime)
    {
      // 画面を指定した色でクリアします
      GraphicsDevice.Clear(Color.CornflowerBlue);

      // 描画に使用する頂点バッファをセット
      GraphicsDevice.SetVertexBuffer(_vertexBuffer);

      // インデックスバッファをセット
      GraphicsDevice.Indices = _indexBuffer;

      // パスの数だけ繰り替えし描画 (といっても BasicEffect は通常１回)
      foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
      {
        // パスの開始
        pass.Apply();

        // インデックスを使用してポリゴンを描画する
        GraphicsDevice.DrawIndexedPrimitives(
            PrimitiveType.TriangleList,
            0,
            0,
            8,
            0,
            12
        );
      }

      // 登録された DrawableGameComponent を描画する
      base.Draw(gameTime);
    }
  }
}
