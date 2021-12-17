import { IconChevronUp, IconLoader } from "@tabler/icons";
import dayjs from "dayjs";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { apiUrl, retries } from "../api";

export default function ClientContractList() {
  const navigate = useNavigate();
  const { clientId } = useParams();
  const [loading, setLoading] = useState(true);
  const [contracts, setContracts] = useState([]);

  useEffect(() => {
    if (!clientId) {
      return;
    }
    const url = apiUrl("ClientContracts");
    url.searchParams.set("clientId", clientId);
    retries(() => fetch(url, { method: "GET" }))
      .then((response) => response.json())
      .then((response) => {
        setContracts(response);
        setLoading(false);
      })
      .catch((err) => console.log(err));
  }, [clientId]);

  return (
    <div className="table-responsive">
      <table className="table card-table table-vcenter text-nowrap datatable table-hover">
        <thead>
          <tr>
            <th className="w-1">
              Code <IconChevronUp />
            </th>
            <th>Title</th>
            <th>Signature Date</th>
          </tr>
        </thead>
        <tbody>
          <tr className={loading ? "" : "d-none"}>
            <td colSpan="3">
              <IconLoader /> Loading...
            </td>
          </tr>
          {contracts.map((contract) => (
            <tr
              key={contract.identifier}
              onClick={() =>
                navigate("/clientcontracts/" + contract.identifier)
              }
            >
              <td>{contract.internalCode}</td>
              <td>{contract.title}</td>
              <td>{dayjs(contract.signatureDate).format("DD MMM YYYY")}</td>
            </tr>
          ))}
          {!loading && contracts.length === 0 && (
            <tr>
              <td colSpan="3">
                <em>No results</em>
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}
