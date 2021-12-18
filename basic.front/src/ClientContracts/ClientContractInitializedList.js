import { useApiClientContracts } from "../api";
import ClientContractList from "./ClientContractList";

export default function ClientContractInitializedList() {
  const [loading, contracts] = useApiClientContracts();
  return <ClientContractList loading={loading} contracts={contracts} />;
}
