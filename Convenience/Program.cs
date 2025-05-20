using Microsoft.EntityFrameworkCore;//データベース操作
using Microsoft.Extensions.DependencyInjection;//依存性注入のためのライブラリ
using Convenience.Data;
using Convenience.Models.Interfaces;
using Convenience.Models.Services;
namespace Convenience
{
    public class Program//アプリケーションの構成、サービスの登録、HTTPリクエストパイプラインの設定を行なう
    {
        public static void Main(string[] args)
        {
            //ASP.NET Coreアプリケーションのビルダーを作成 →アプリケーションの設定を行う準備が整う
            var builder = WebApplication.CreateBuilder(args);

            //DBコンテキストの設定
            builder.Services.AddDbContext<ConvenienceContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("ConvenienceContext") ?? throw new InvalidOperationException("Connection string 'ConvenienceContext' not found.")));
            /*
             * AddDbContext<ConvenienceContext>: ConvenienceContextというDBコンテキストを依存性注入コンテナに登録
             * 　→このコンテキストは、Entity Framework Coreを使用してDBとやり取りを行うために使われる
             * UseNpgsql: PostgreSQLデータベースを使用するための設定
             * 　接続文字列（ConvenienceContext）をappsettings.jsonなどから読み込む
             * 　→接続文字列が見つからない場合、例外をスロー
             */

            // Add services to the container.
            //MVCの設定（コントローラとビュー）
            //　→この設定により、コントローラがリクエストを処理し、ビューがレンダリングされるようになる
            builder.Services.AddControllersWithViews();


            //DIコンテナのサービス登録
            //Dependency Injection（依存性の注入）
            //サービス用
            /*
             * 依存性注入コンテナにIChumonServiceインターフェースと、その実装であるChumonServiceクラスを登録
             * このサービスはスコープ付きで登録されるため、リクエストごとに新しいインスタンスが作成される
             * 　→IChumonServiceが要求されるたびに、ChumonServiceが提供される
             */
            builder.Services.AddScoped<IChumonService, ChumonService>();

            //プロパティ用

            //アプリケーションの構築と設定
            //WebApplicationオブジェクトを構築し、アプリケーションを実行できる状態にする
            //これ以降、HTTPリクエストの処理やルーティングなどを設定
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //リクエストパイプラインの設定
            if (!app.Environment.IsDevelopment())//現在の実行環境が「開発環境」かどうかを判定
            {
                app.UseExceptionHandler("/Home/Error");//エラーが発生した場合、エラーページ（/Home/Error）にリダイレクト
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();//HTTP Strict Transport Security (HSTS)を有効にし、ブラウザにHTTPSのみで接続するように指示。本番環境で有効になる
            }

            app.UseHttpsRedirection();//HTTPリクエストをHTTPSにリダイレクト　→セキュアな通信が強制される
            app.UseStaticFiles();//静的ファイル（画像、CSS、JavaScriptなど）を提供するための設定　→wwwrootフォルダ内の静的ファイルがクライアントに配信される

            app.UseRouting();//リクエストのルーティングを設定　→URLに基づいて適切なコントローラやアクションが選ばれる

            app.UseAuthorization();//認証（ユーザーの権限の確認）を有効にする　→特定のアクションやリソースにアクセスするための権限が必要になる

            app.MapControllerRoute( //アプリケーションで使用するルーティングパターンを設定
                name: "default",
                pattern: "{controller=Chumon}/{action=KeyInput}/{id?}");

            app.Run();//アプリケーションを実行　→アプリケーションがWebサーバーとして起動し、リクエストの受け付けを開始
        }
    }
}
