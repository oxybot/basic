import { IconChevronUp, IconLoader } from "@tabler/icons";
import dayjs from "dayjs";
import { useNavigate } from "react-router-dom";

export default function AgreementList({ loading, agreements }) {
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
          {agreements.map((agreement) => (
            <tr
              key={agreement.identifier}
              onClick={() => navigate("/agreements/" + agreement.identifier)}
            >
              <td>{agreement.internalCode}</td>
              <td>{agreement.title}</td>
              <td>{dayjs(agreement.signatureDate).format("DD MMM YYYY")}</td>
            </tr>
          ))}
          {!loading && agreements.length === 0 && (
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
