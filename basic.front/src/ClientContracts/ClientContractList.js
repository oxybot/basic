import { IconChevronUp, IconLoader } from "@tabler/icons";
import dayjs from "dayjs";
import { useNavigate } from "react-router-dom";

export default function ClientContractList({ loading, contracts }) {
  const navigate = useNavigate();

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
