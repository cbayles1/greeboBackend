import psycopg2, dotenv, os, csv, time

dotenv.load_dotenv()
conStr = os.getenv('CONNECTION_STRING')

#                                       TODO: CURRENTLY NO SQL INJECTION PREVENTION

def main():
    conn = psycopg2.connect(conStr)
    cur = conn.cursor()
    csvFile = "./Models/flavors_of_cacao.csv"
    
    chocolateBars = Table(conn, cur, "chocolate_bars", [
                          ("company VARCHAR(255)",),
                          ("specific_origin VARCHAR(255)",),
                          ("recency INT", lambda a : int(a)), 
                          ("review_year INT", lambda a : int(a)),
                          ("cacao_percent INT NOT NULL", lambda a : a[:-1]), 
                          ("company_location VARCHAR(255)",),
                          ("rating REAL", lambda a : float(a)), 
                          ("bean_type VARCHAR(255)",),
                          ("broad_origin VARCHAR(255)",),])
    chocolateBars.populate(conn, cur, csvFile)

    cur.close()
    conn.close()

class Table:
    tableName = None
    
    def __init__(self, conn, cur, tableName, columns):
        self.tableName = tableName
        self.columns = columns
        self.up(conn, cur)
    
    def up(self, conn, cur): # create table
        formattedCols = ", ".join(a[0] for a in self.columns)
        broadcastCmdToDB(conn, cur, f"CREATE TABLE IF NOT EXISTS {self.tableName} (id SERIAL, {formattedCols});", 
            f"Could not create table {self.tableName}.")
            
    def down(self, conn, cur): # delete table
        broadcastCmdToDB(conn, cur, f"DROP TABLE {self.tableName};", f"Could not delete table {self.tableName}.")
       
    def populate(self, conn, cur, csvFile): # fill table with data from csv file
        print("\nPopulating table...")
        start = time.perf_counter()
        with open(csvFile, 'r') as f:
            reader = csv.reader(f)
            next(reader)
            headers = [col[0].split()[0] for col in self.columns]
            formattedHeaders = ", ".join(headers)
            for i, row in enumerate(reader):
                formattedRow, escapedValues = self.formatCsvRow(row, i)
                insertQuery = f"INSERT INTO {self.tableName} ({formattedHeaders}) VALUES {formattedRow}"
                if len(escapedValues) > 0: cur.execute(insertQuery, escapedValues)
                else: cur.execute(insertQuery)
        conn.commit()
        end = time.perf_counter()
        print(f"Done. {end - start:0.4f} seconds elapsed.")
        
    def formatCsvRow(self, row):
        new = f"("
        escapedValues = tuple()
        for i, element in enumerate(row):
            formattedElement = element
            if len(self.columns[i]) > 1 and self.columns[i][1]:
                formattedElement = self.columns[i][1](formattedElement)
                new += f"{formattedElement}, "
            else:
                new += f"%s"
                if i < len(row) - 1: new += ", "
                escapedValues += (element,)
        new += ")"
        return new, escapedValues 
                    
def broadcastCmdToDB(conn, cur, cmd, exception): # send query to DB without expecting a response
    try:
        cur.execute(cmd)
        conn.commit()
    except:
        raise Exception(exception)
        
if __name__ == "__main__":
    main()