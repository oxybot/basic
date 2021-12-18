import { useApiAgreements } from "../api";
import AgreementList from "./AgreementList";

export default function AgreementInitializedList() {
  const [loading, agreements] = useApiAgreements();
  return <AgreementList loading={loading} agreements={agreements} />;
}
