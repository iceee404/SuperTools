import "./App.css";
import { useEffect, useState } from "react";

type Location = {
  id: string;
  locationName: string;
  locationType: string;
  storeNumber?: string;
  address?: string;
};

type TransferLog = {
  id: string;
  printerID: string;
  fromLocationID: string;
  toLocationID: string;
  transferDate: string;
  transferredBy: string;
  transferReason: string;
  transferStatus?: string;
  notes?: string;
  fromLocation: Location;
  toLocation: Location;
};

type Printer = {
  id: string;
  brand: string;
  modelType: string;
  tonerModel: string;
  serialNumber: string;
  specification: string;
  locationID: string;
  status: string;
  notes?: string;
  warrantyExpiryDate?: string;
  purchaseDate?: string;
  location?: Location | null;
  transferLogs?: TransferLog[];
};

function App() {
  const [printers, setPrinters] = useState<Printer[]>([]);
  const [transferLogs, setTransferLogs] = useState<TransferLog[]>([]);
  const [locations, setLocations] = useState<Location[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [activeTab, setActiveTab] = useState<'printers' | 'transfers' | 'locations'>('printers');

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [printersResponse, transferLogsResponse] = await Promise.all([
          fetch("http://localhost:5209/api/printer"),
          fetch("http://localhost:5209/api/transferlog")
        ]);

        if (!printersResponse.ok || !transferLogsResponse.ok) {
          throw new Error("网络请求失败");
        }

        const [printersData, transferLogsData] = await Promise.all([
          printersResponse.json(),
          transferLogsResponse.json()
        ]);

        setPrinters(printersData);
        setTransferLogs(transferLogsData);

        // 从打印机数据中提取位置信息
        const uniqueLocations = printersData
          .filter((p: Printer) => p.location)
          .map((p: Printer) => p.location)
          .filter((location: Location, index: number, self: Location[]) =>
            index === self.findIndex(l => l.id === location.id));
        setLocations(uniqueLocations);

      } catch (err) {
        setError(err instanceof Error ? err.message : "获取数据失败");
        console.error("Error fetching data:", err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  if (loading) return <div className="loading">加载中...</div>;
  if (error) return <div className="error">错误: {error}</div>;

  const renderPrintersTab = () => (
    <div>
      <h2>打印机列表</h2>
      <table>
        <thead>
          <tr>
            <th>品牌</th>
            <th>型号</th>
            <th>墨粉型号</th>
            <th>序列号</th>
            <th>规格</th>
            <th>状态</th>
            <th>位置</th>
            <th>位置类型</th>
            <th>备注</th>
          </tr>
        </thead>
        <tbody>
          {printers.map((printer) => (
            <tr key={printer.id}>
              <td>{printer.brand}</td>
              <td>{printer.modelType}</td>
              <td>{printer.tonerModel}</td>
              <td>{printer.serialNumber}</td>
              <td>{printer.specification}</td>
              <td>{printer.status}</td>
              <td>{printer.location?.locationName || "未分配"}</td>
              <td>{printer.location?.locationType || "-"}</td>
              <td>{printer.notes || "-"}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );

  const renderTransfersTab = () => (
    <div>
      <h2>转移记录</h2>
      <table>
        <thead>
          <tr>
            <th>转移日期</th>
            <th>打印机ID</th>
            <th>从</th>
            <th>到</th>
            <th>转移原因</th>
            <th>状态</th>
            <th>执行人</th>
            <th>备注</th>
          </tr>
        </thead>
        <tbody>
          {transferLogs.map((log) => (
            <tr key={log.id}>
              <td>{new Date(log.transferDate).toLocaleDateString()}</td>
              <td>{log.printerID}</td>
              <td>{log.fromLocation.locationName}</td>
              <td>{log.toLocation.locationName}</td>
              <td>{log.transferReason}</td>
              <td>{log.transferStatus || "-"}</td>
              <td>{log.transferredBy}</td>
              <td>{log.notes || "-"}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );

  const renderLocationSummary = () => {
    const locationStats = locations.map(location => {
      const locationPrinters = printers.filter(p => p.location?.id === location.id);
      return {
        ...location,
        printerCount: locationPrinters.length,
        printers: locationPrinters
      };
    });

    return (
      <div>
        <h2>位置统计</h2>
        {locationStats.map(location => (
          <div key={location.id} style={{ margin: '20px 0', padding: '15px', border: '1px solid #ccc' }}>
            <h3>{location.locationName} ({location.locationType})</h3>
            <p>打印机数量: {location.printerCount}</p>
            {location.storeNumber && <p>门店编号: {location.storeNumber}</p>}
            {location.address && <p>地址: {location.address}</p>}

            <h4>打印机详情:</h4>
            <table style={{ width: '100%', marginTop: '10px' }}>
              <thead>
                <tr>
                  <th>品牌</th>
                  <th>型号</th>
                  <th>序列号</th>
                  <th>状态</th>
                </tr>
              </thead>
              <tbody>
                {location.printers.map(printer => (
                  <tr key={printer.id}>
                    <td>{printer.brand}</td>
                    <td>{printer.modelType}</td>
                    <td>{printer.serialNumber}</td>
                    <td>{printer.status}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        ))}
      </div>
    );
  };

  return (
    <>
      <h1>打印机管理系统</h1>

      <div style={{ marginBottom: '20px' }}>
        <button
          onClick={() => setActiveTab('printers')}
          style={{
            marginRight: '10px',
            backgroundColor: activeTab === 'printers' ? '#007bff' : '#f8f9fa',
            color: activeTab === 'printers' ? 'white' : 'black',
            padding: '10px 20px',
            border: '1px solid #ccc',
            borderRadius: '4px'
          }}
        >
          打印机列表
        </button>
        <button
          onClick={() => setActiveTab('transfers')}
          style={{
            marginRight: '10px',
            backgroundColor: activeTab === 'transfers' ? '#007bff' : '#f8f9fa',
            color: activeTab === 'transfers' ? 'white' : 'black',
            padding: '10px 20px',
            border: '1px solid #ccc',
            borderRadius: '4px'
          }}
        >
          转移记录
        </button>
        <button
          onClick={() => setActiveTab('locations')}
          style={{
            backgroundColor: activeTab === 'locations' ? '#007bff' : '#f8f9fa',
            color: activeTab === 'locations' ? 'white' : 'black',
            padding: '10px 20px',
            border: '1px solid #ccc',
            borderRadius: '4px'
          }}
        >
          位置统计
        </button>
      </div>

      {activeTab === 'printers' && renderPrintersTab()}
      {activeTab === 'transfers' && renderTransfersTab()}
      {activeTab === 'locations' && renderLocationSummary()}
    </>
  );
}

export default App;
