namespace Sample.Core
open SQLite
open System.IO

type IDatabasePath = 
    abstract member Directory: string
    
type IDatabase = 
    abstract member Run: (SQLiteAsyncConnection -> 'T) -> 'T
    
type Database(db:IDatabasePath) =

    let dbName = "database.db3"

    let connection = 
        let dbPath = Path.Combine(db.Directory, dbName)
        new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.ReadWrite ||| SQLiteOpenFlags.Create ||| SQLiteOpenFlags.FullMutex)


    interface IDatabase with 
        member this.Run f = 
            f connection


